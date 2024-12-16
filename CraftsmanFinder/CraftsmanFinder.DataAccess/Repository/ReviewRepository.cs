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
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<double> GetAverageRatingByUserIdAsync(string userId)
        {
            var reviews = await _context.reviews
                .Where(review => review.ApplicationUserId == userId)
                .ToListAsync();

            if (reviews.Any())
            {
                return reviews.Average(review => review.Rating);
            }

            return 0;
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId)
        {
            return await _context.Set<Review>()
            .Where(review => review.ApplicationUserId == userId)
            .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetSortedReviewsByRatingAsync(bool ascending = true)
        {
            var reviewsQuery = _context.reviews.AsQueryable();

            if (ascending)
            {
                reviewsQuery = reviewsQuery.OrderBy(review => review.Rating); 
            }
            else
            {
                reviewsQuery = reviewsQuery.OrderByDescending(review => review.Rating);
            }

            return await reviewsQuery.ToListAsync();
        }
    }
}
