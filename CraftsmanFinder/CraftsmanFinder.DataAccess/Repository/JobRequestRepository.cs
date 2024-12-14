using CraftsmanFinder.DataAccess.Data;
using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Entities.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository
{
    public class JobRequestRepository : GenericRepository<JobRequest> , IJobRequestRepository
    {
        private readonly AppDbContext _context;

        public JobRequestRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<JobRequestDetailsViewModel> GetDetailsAsync(int jobRequestId)
        {
            var jobRequest = await _context.jobRequests
                .Include(j => j.Attachment)
                .Include(j => j.Category)
                .Include(j => j.offers) 
                    .ThenInclude(o => o.ApplicationUser) 
                .FirstOrDefaultAsync(j => j.Id == jobRequestId);

            if (jobRequest == null)
            {
                return null;
            }

            return new JobRequestDetailsViewModel
            {
                Id = jobRequest.Id,
                Title = jobRequest.Title,
                Description = jobRequest.Description,
                Location = jobRequest.Location,
                RightTime = jobRequest.RightTime,
                Status = jobRequest.Status,
                ApplicationUserId = jobRequest.ApplicationUserId,
                CategoryName = jobRequest.Category?.Name,
                CategoryId = jobRequest.CategoryId,
                Attachments = jobRequest.Attachment?.Select(a => a.FilePath).ToList() ?? new List<string>(),
                Offers = jobRequest.offers?.Select(o => new OfferViewModel
                {
                    Id = o.Id,
                    JobRequestId = o.JobRequestId,
                    UserName = o.ApplicationUser?.Name,
                    ApplicationUserId = o.ApplicationUserId,
                    Price = o.Price,
                    status = o.status,
                    NegotiationDetails = o.NegotiationDetails
                }).ToList() ?? new List<OfferViewModel>()
            };
        }


        public async Task<IEnumerable<JobRequestListViewModel>> GetByCategoryAndStatusAsync(int categoryId, bool status = false)
        {
            return await _context.jobRequests
                .Where(jr => jr.CategoryId == categoryId && jr.Status == status)
                .Select(jr => new JobRequestListViewModel
                {
                    Id = jr.Id,
                    Title = jr.Title,
                    Description = jr.Description,
                    Location = jr.Location,
                    Created = jr.Created,
                    UserName = jr.ApplicationUser != null ? jr.ApplicationUser.UserName : string.Empty,
                    ImagePath = _context.Attachments
                        .Where(a => a.JobRequestId == jr.Id)
                        .Select(a => a.FilePath)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }


        public async Task UpdateAsync(JobRequest jobRequest)
        {
            var existingJobRequest = await _context.jobRequests
                                                      .Include(j => j.Attachment)
                                                      .FirstOrDefaultAsync(j => j.Id == jobRequest.Id);
            if (existingJobRequest == null)
            {
                throw new KeyNotFoundException($"JobRequest with Id {jobRequest.Id} not found.");
            }

            existingJobRequest.Title = jobRequest.Title;
            existingJobRequest.Description = jobRequest.Description;
            existingJobRequest.Location = jobRequest.Location;
            existingJobRequest.RightTime = jobRequest.RightTime;
            existingJobRequest.Status = jobRequest.Status;
            existingJobRequest.CategoryId = jobRequest.CategoryId;

            await _context.SaveChangesAsync();
        }

    }
}
