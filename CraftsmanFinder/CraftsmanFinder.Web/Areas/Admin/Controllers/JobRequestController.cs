using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftsmanFinder.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole}")]
    public class JobRequestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobRequestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var jobRequests = await _unitOfWork.JobRequests.GetAllAsync(Includeword: "ApplicationUser,Category");
            return View(jobRequests);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _unitOfWork.JobRequests.GetFirstorDefaultsync(x => x.Id == id);
            if (review != null)
            {
                await _unitOfWork.JobRequests.DeleteJobRequestWithDependenciesAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
