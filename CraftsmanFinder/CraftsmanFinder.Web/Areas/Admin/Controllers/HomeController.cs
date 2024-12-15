using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftsmanFinder.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole}")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Craftsmen = await _unitOfWork.ApplicationUsers.GetUsersCountByRoleAsync(SD.CraftsmenRole);
            ViewBag.HomeOwner = await _unitOfWork.ApplicationUsers.GetUsersCountByRoleAsync(SD.HomeOwnerRole);
            ViewBag.jobs = (await _unitOfWork.JobRequests.GetAllAsync()).Count();
            var jobRequests = (await _unitOfWork.JobRequests.GetAllAsync(Includeword :"Category"))
                                    .OrderByDescending(x => x.Id)
                                    .Take(4)
                                    .ToList();

            return View(jobRequests);

        }
    }
}
