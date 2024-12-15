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
    public class NotificationRepository : GenericRepository<Notification> , INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<List<Notification>> GetNotificationsByUserIdAsync(string applicationUserId)
        {
            return await _context.notifications
                .Where(n => n.ApplicationUserId == applicationUserId)
                .ToListAsync();
        }

        public async Task MarkAsWatchedAsync(int notificationId)
        {
            var notification = await _context.notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsWatched = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
