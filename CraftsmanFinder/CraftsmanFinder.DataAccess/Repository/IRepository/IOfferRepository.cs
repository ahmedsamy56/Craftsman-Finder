using CraftsmanFinder.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository.IRepository
{
    public interface IOfferRepository : IGenericRepository<Offer>
    {
        Task UpdateAsync(Offer offer);
        Task<IEnumerable<Offer>> GetOffersByUserIdAsync(string userId);
        Task<IEnumerable<Offer>> GetAcceptedOffersAsync();
        Task<IEnumerable<Offer>> GetSortedOffersByPriceAsync(bool ascending = true);
    }

}
