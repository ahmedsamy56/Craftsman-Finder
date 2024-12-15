using CraftsmanFinder.DataAccess.Repository;
using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace CraftsmanFinder.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.AdminRole}")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, IFormFile LogoFile)
        {
            if (ModelState.IsValid)
            {
                if (LogoFile != null)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Category");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + LogoFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await LogoFile.CopyToAsync(fileStream);
                    }

                    category.Logo = "/uploads/Category/" + uniqueFileName;
                }

                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _unitOfWork.Categories.GetFirstorDefaultsync(x=>x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category, IFormFile? LogoFile)
        {
            if (ModelState.IsValid)
            {
                var old = await _unitOfWork.Categories.GetFirstorDefaultsync(x => x.Id == category.Id);
                if (LogoFile != null)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Category");
                    Directory.CreateDirectory(uploadsFolder); 
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + LogoFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await LogoFile.CopyToAsync(fileStream);
                    }

                   
                    if (!string.IsNullOrEmpty(old.Logo))
                    {
                        string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", old.Logo.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    category.Logo = "/uploads/Category/" + uniqueFileName;
                }
                else
                {
                    category.Logo = old.Logo;
                }

                await _unitOfWork.Categories.UpdateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _unitOfWork.Categories.GetFirstorDefaultsync(x=>x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            
            if (!string.IsNullOrEmpty(category.Logo))
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", category.Logo.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            await _unitOfWork.Categories.DeleteAsync(category);

            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
