using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CraftsmanFinder.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole}")]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userid = claim.Value;
            var users = await _unitOfWork.ApplicationUsers.GetAllAsync(x => x.Id != userid, Includeword: "Category");

            return View(users);
        }

        public async Task<IActionResult> LockUnlock(string? id)
        {
            var user = await _unitOfWork.ApplicationUsers.GetFirstorDefaultsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddMonths(3);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }

            await _unitOfWork.SaveAsync();
            return RedirectToAction("Index", "Users", new { area = "Admin" });
        }
    }
}
