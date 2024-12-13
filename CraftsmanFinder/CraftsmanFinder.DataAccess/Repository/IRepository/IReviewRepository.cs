using CraftsmanFinder.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository.IRepository
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId);
        Task<double> GetAverageRatingByUserIdAsync(string userId);
        Task<IEnumerable<Review>> GetSortedReviewsByRatingAsync(bool ascending = true);

    }

}
