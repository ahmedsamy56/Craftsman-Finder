using CraftsmanFinder.DataAccess.Data;
using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository
{
    public class JobRequestAttachmentRepository : GenericRepository<JobRequestAttachment> , IJobRequestAttachmentRepository
    {
        private readonly AppDbContext _context;

        public JobRequestAttachmentRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<JobRequestAttachment>> GetAttachmentsByJobRequestIdAsync(int jobRequestId)
        => await _context.Set<JobRequestAttachment>()
            .Where(attachment => attachment.JobRequestId == jobRequestId)
            .ToListAsync();
    }
}
