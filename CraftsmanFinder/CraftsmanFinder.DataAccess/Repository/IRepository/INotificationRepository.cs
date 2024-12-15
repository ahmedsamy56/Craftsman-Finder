using CraftsmanFinder.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository.IRepository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<List<Notification>> GetNotificationsByUserIdAsync(string applicationUserId);
        Task MarkAsWatchedAsync(int notificationId);
    }
}
