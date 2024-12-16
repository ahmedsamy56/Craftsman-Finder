using CraftsmanFinder.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace CraftsmanFinder.Web.Areas.HomeOwner.Controllers
{
    [Area("HomeOwner")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(int id)
        {
            var category = await _unitOfWork.Categories.GetFirstorDefaultsync(x=>x.Id == id);
            if (category == null) {
                return View("/Views/Shared/NotFound.cshtml");
            }
            var JobRequestList = await _unitOfWork.Categories.GetByCategoryAsync(id);
            ViewBag.categoryName = category.Name;


            return View(JobRequestList);
        }
    }
}
