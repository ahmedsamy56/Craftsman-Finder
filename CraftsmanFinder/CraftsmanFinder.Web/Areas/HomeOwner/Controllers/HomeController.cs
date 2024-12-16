using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftsmanFinder.Web.Areas.HomeOwner.Controllers
{
    [Area("HomeOwner")]
    [Authorize(Roles = $"{SD.HomeOwnerRole}")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return View(categories);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Landing()
        {
            var Categories = await _unitOfWork.Categories.GetAllAsync();
            return View(Categories);
        }
    }
}
