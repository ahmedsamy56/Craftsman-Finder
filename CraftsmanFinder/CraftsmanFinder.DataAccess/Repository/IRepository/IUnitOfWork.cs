using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUsers { get; }
        ICategoryRepository Categories { get; }
        ICertificateRepository Certificates { get; }
        IJobRequestRepository JobRequests { get; }
        IJobRequestAttachmentRepository Attachments { get; }
        IOfferRepository Offers { get; }
        IReviewRepository Reviews { get; }
        INotificationRepository Notifications { get; }

        Task SaveAsync();

    }
}
