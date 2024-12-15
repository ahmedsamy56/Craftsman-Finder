using CraftsmanFinder.DataAccess.Data;
using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository
{
    public class OfferRepository : GenericRepository<Offer>, IOfferRepository
    {
        private readonly AppDbContext _context;
        public OfferRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Offer>> GetAcceptedOffersAsync()
        {
            return await _context.offers
             .Where(offer => offer.status == true)
             .ToListAsync();
        }

        public async Task<IEnumerable<Offer>> GetOffersByUserIdAsync(string userId)
        {
            return await _context.offers
            .Where(offer => offer.ApplicationUserId == userId)
            .ToListAsync();
        }

        public async Task<IEnumerable<Offer>> GetSortedOffersByPriceAsync(bool ascending = true)
        {
            var offersQuery = _context.Set<Offer>().AsQueryable();

            if (ascending)
            {
                offersQuery = offersQuery.OrderBy(offer => offer.Price);
            }
            else
            {
                offersQuery = offersQuery.OrderByDescending(offer => offer.Price);
            }

            return await offersQuery.ToListAsync();
        }

        public async Task UpdateAsync(Offer offer)
        {
            var existingOffer = await _context.offers.FindAsync(offer.Id);
            if (existingOffer == null)
            {
                throw new Exception("Offer not found");
            }
            if (existingOffer.status == true)
            {
                throw new InvalidOperationException("Cannot update the offer because the offer is already accepted.");
            }

            existingOffer.NegotiationDetails = offer.NegotiationDetails;
            existingOffer.Price = offer.Price; ;
            await _context.SaveChangesAsync();
        }


        public async Task AcceptOfferAsync(int offerId)
        {
  
            var offer = await _context.offers.Include(o => o.JobRequest).FirstOrDefaultAsync(o => o.Id == offerId);

            if (offer == null)
            {
                throw new Exception("Offer not found.");
            }

            offer.status = true;

            if (offer.JobRequest != null)
            {
                offer.JobRequest.Status = true;
                _context.jobRequests.Update(offer.JobRequest);
            }
            _context.offers.Update(offer);
            var notifiCraftsman = new Notification
            {
                Title = "Your offer has been accepted.",
                Type = NotificationType.Accepted,
                Content = "I have good news, your job application has just been approved. Now contact HomeOwner and do your job.",
                Sender = NotificationSender.System,
                NeedAction = true,
                Link = $"/HomeOwner/JobRequest/JobRequestDetails/{offer.JobRequestId}",
                IsWatched = false,
                ApplicationUserId = offer.ApplicationUserId

            };

            var notifiHomeOwner = new Notification
            {
                Title = "Submit a review of your last job.",
                Type = NotificationType.Review,
                Content = "You just accepted a quote from a Craftsman, wait until after the job is done and the Craftsman gives you a fair review and states everything according to the guidelines.",
                Sender = NotificationSender.System, 
                NeedAction = true,
                Link = $"/HomeOwner/Review/Create?applicationUserId={offer.ApplicationUserId}&jobRequestId={offer.JobRequestId}",
                IsWatched = false,
                ApplicationUserId = offer.JobRequest.ApplicationUserId

            };
            await _context.AddAsync(notifiCraftsman);
            await _context.AddAsync(notifiHomeOwner);
            await _context.SaveChangesAsync();
        }
    }
}
