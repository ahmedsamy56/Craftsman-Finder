using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CraftsmanFinder.Web.Areas.HomeOwner.Controllers
{
    [Area("HomeOwner")]
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public NotificationController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager = null)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> GetNotifications()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = await _unitOfWork.Notifications.GetNotificationsByUserIdAsync(userId);

            return Json(notifications);
        }
        public async Task<IActionResult> ViewNotification(int id)
        {
            
            var notification = await _unitOfWork.Notifications.GetFirstorDefaultsync(x=>x.Id == id);

            if (notification == null)
            {
                return NotFound();
            }
            await _unitOfWork.Notifications.MarkAsWatchedAsync(notification.Id);
            return View(notification);
        }

    }
}
