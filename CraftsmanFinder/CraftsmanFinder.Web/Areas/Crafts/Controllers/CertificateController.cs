using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace CraftsmanFinder.Web.Areas.Crafts.Controllers
{
    [Area("Crafts")]
    [Authorize(Roles = $"{SD.CraftsmenRole}")]
    public class CertificateController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CertificateController(IUnitOfWork unitOfWork , UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Add(string certificateDescription, IFormFile certificateImage)
        {
            var userId = _userManager.GetUserId(User);
            if (certificateImage != null && certificateImage.Length > 0 && certificateDescription != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "certificates");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(certificateImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await certificateImage.CopyToAsync(stream);
                }

                var ImagePath = Path.Combine("uploads", "certificates", fileName);

               
                var newCertificate = new Certificate
                {
                    FilePath = ImagePath,
                    Description = certificateDescription,
                    ApplicationUserId = userId,
                };

                await _unitOfWork.Certificates.AddAsync(newCertificate);

                await _unitOfWork.SaveAsync();
                return RedirectToAction("Index", "Profile", new { Area = "Crafts", id = userId });

            }

            return RedirectToAction("Index", "Profile", new { Area = "Crafts", id = userId });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var certificate = await _unitOfWork.Certificates.GetFirstorDefaultsync(x => x.Id == id);

            if (certificate == null)
            {
                return NotFound();
            }


            if (certificate.ApplicationUserId != userId)
            {
                return Forbid(); 
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", certificate.FilePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            await _unitOfWork.Certificates.DeleteAsync(certificate);
            await _unitOfWork.SaveAsync();


            return RedirectToAction("Index", "Profile", new { Area = "Crafts", id = userId });


        }



    }
}
