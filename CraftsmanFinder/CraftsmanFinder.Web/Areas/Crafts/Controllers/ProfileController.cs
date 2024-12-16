using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Entities.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using CraftsmanFinder.Utilities;
using Microsoft.AspNetCore.Authorization;


namespace CraftsmanFinder.Web.Areas.Crafts.Controllers
{
    [Area("Crafts")]
    public class ProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProfileController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string id)
        {

            var user = await _unitOfWork.ApplicationUsers.GetUserWithAllDetailsAsync(id);
            if (user == null)
                return View("/Views/Shared/NotFound.cshtml");

            var averageRating = await _unitOfWork.Reviews.GetAverageRatingByUserIdAsync(id);
            var totalReviews = user.Reviews?.Count ?? 0;
            var viewModel = new ProfileViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                ProfileImagePath = user.ProfileImagePath,
                AboutMe = user.AboutMe,
                CategoryName = user.Category?.Name ?? "Unknown",
                AverageRating = Math.Round(averageRating, 1),
                TotalReviews = totalReviews,
                Certificates = user.certificates?.ToList() ?? new List<Certificate>(),
                Reviews = user.Reviews?.ToList() ?? new List<Review>()
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = $"{SD.CraftsmenRole}")]
        public async Task<IActionResult> CompleteProfile()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });

           
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _unitOfWork.ApplicationUsers.GetFirstorDefaultsync(x=>x.Id == userId);
            var viewModel = new CompleteProfileViewModel
            {
                AboutMe = user?.AboutMe
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.CraftsmenRole}")]
        public async Task<IActionResult> CompleteProfile(CompleteProfileViewModel model, List<IFormFile> certificateImages, List<string> descriptions)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _unitOfWork.ApplicationUsers.GetFirstorDefaultsync(x=>x.Id == userId);
            if (user != null)
            {
                user.CategoryId = model.CategoryId;
                user.AboutMe = model.AboutMe;
            }
            var updateResult = await _userManager.UpdateAsync(user);

            for (int i = 0; i < certificateImages.Count; i++)
            {
                var file = certificateImages[i];
                var description = descriptions[i];

                if (file != null && file.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "certificates");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var ImagePath = Path.Combine("uploads", "certificates", fileName);

                    var newCertificate = new Certificate
                    {
                        FilePath = ImagePath,
                        Description = description,
                        ApplicationUserId = userId,
                    };
                    await _unitOfWork.Certificates.AddAsync(newCertificate);
                }
            }

            await _unitOfWork.SaveAsync();

            return RedirectToAction("Index", "Home");
        }


    }
}
