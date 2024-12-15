using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Entities.ViewModel;
using CraftsmanFinder.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CraftsmanFinder.Web.Areas.HomeOwner.Controllers
{
    [Area("HomeOwner")]
    [Authorize(Roles = $"{SD.HomeOwnerRole}")]
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public ReviewController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: /Review/Create
        [HttpGet]
        public async Task<IActionResult> Create(string applicationUserId, int jobRequestId)
        {
            if (string.IsNullOrEmpty(applicationUserId) || jobRequestId <= 0)
            {
                return BadRequest("Invalid ApplicationUserId or JobRequestId.");
            }
            var userId = _userManager.GetUserId(User);
            var jobRequestExists = await _unitOfWork.JobRequests.GetFirstorDefaultsync(j => j.Id == jobRequestId);
            if (jobRequestExists == null || userId != jobRequestExists.ApplicationUserId)
            {
                return NotFound("Invalid");
            }

            var model = new ReviewViewModel
            {
                ApplicationUserId = applicationUserId,
                JobRequestId = jobRequestId
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

      
            var review = new Review
            {
                ApplicationUserId = model.ApplicationUserId,
                JobRequestId = model.JobRequestId,
                Rating = model.Rating,
                Comment = model.Comment
            };

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("Index", "Home", new { area = "HomeOwner"});

        }



    }
}
