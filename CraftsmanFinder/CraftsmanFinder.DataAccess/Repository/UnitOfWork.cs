using CraftsmanFinder.DataAccess.Data;
using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IApplicationUserRepository ApplicationUsers {  get; private set; }

        public ICategoryRepository Categories { get; private set; }

        public ICertificateRepository Certificates { get; private set; }

        public IJobRequestRepository JobRequests { get; private set; }

        public IJobRequestAttachmentRepository Attachments { get; private set; }

        public IOfferRepository Offers { get; private set; }

        public IReviewRepository Reviews { get; private set; }
        public INotificationRepository Notifications { get; private set; }

        public UnitOfWork(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            ApplicationUsers = new ApplicationUserRepository(context, userManager);
            Categories = new CategoryRepository(context);
            Certificates = new CertificateRepository(context);
            JobRequests = new JobRequestRepository(context);
            Attachments = new JobRequestAttachmentRepository(context);
            Offers = new OfferRepository(context);
            Reviews = new ReviewRepository(context); 
            Notifications = new NotificationRepository(context);

        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveAsync()
        {
          await _context.SaveChangesAsync();
        }
    }
}
