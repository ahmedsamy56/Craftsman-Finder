using CraftsmanFinder.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository.IRepository
{
    public interface IJobRequestAttachmentRepository : IGenericRepository<JobRequestAttachment>
    {
        Task<IEnumerable<JobRequestAttachment>> GetAttachmentsByJobRequestIdAsync(int jobRequestId);

    }
}
