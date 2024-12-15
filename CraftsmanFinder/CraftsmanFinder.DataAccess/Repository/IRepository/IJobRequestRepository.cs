using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository.IRepository
{
    public interface IJobRequestRepository : IGenericRepository<JobRequest>
    {
        Task UpdateAsync(JobRequest jobRequest);
        Task<IEnumerable<JobRequestListViewModel>> GetByCategoryAndStatusAsync(int categoryId, bool status = false);
        Task<JobRequestDetailsViewModel> GetDetailsAsync(int jobRequestId);
        Task DeleteJobRequestWithDependenciesAsync(int jobRequestId);

    }
}
