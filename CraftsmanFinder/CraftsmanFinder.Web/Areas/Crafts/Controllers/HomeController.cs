using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CraftsmanFinder.Web.Areas.Crafts.Controllers
{
    [Area("Crafts")]
    [Authorize(Roles = $"{SD.CraftsmenRole}")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _unitOfWork.ApplicationUsers.GetFirstorDefaultsync(x=>x.Id ==  userId);
            if(user == null)
            {
                return View("/Views/Shared/NotFound.cshtml");
            }
            if (user.CategoryId == null)
            {
                return RedirectToAction("CompleteProfile");
            }

            var jobRequests = await _unitOfWork.JobRequests.GetByCategoryAndStatusAsync(user.CategoryId.Value);
            return View(jobRequests);
        }

        public IActionResult CompleteProfile()
        {
            return View();
        }
    }
}
