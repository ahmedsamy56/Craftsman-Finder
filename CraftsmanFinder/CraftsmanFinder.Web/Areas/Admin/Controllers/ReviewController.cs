using CraftsmanFinder.DataAccess.Repository;
using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftsmanFinder.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole}")]
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync(Includeword: "ApplicationUser");
            return View(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _unitOfWork.Reviews.GetFirstorDefaultsync(x=>x.Id == id);
            if (review != null)
            {
                await _unitOfWork.Reviews.DeleteAsync(review);
                await _unitOfWork.SaveAsync();
            }
           
            return RedirectToAction(nameof(Index));
        }
    }
}
