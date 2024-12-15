using CraftsmanFinder.DataAccess.Data;
using CraftsmanFinder.DataAccess.Repository;
using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Entities.ViewModel;
using CraftsmanFinder.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Security.Claims;

namespace CraftsmanFinder.Web.Areas.HomeOwner.Controllers
{
    [Area("HomeOwner")]
    [Authorize(Roles = $"{SD.HomeOwnerRole}")]
    public class JobRequestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobRequestController(IUnitOfWork IUnitOfWork)
        {
            _unitOfWork = IUnitOfWork;
        }

        [AllowAnonymous]
        public async Task<IActionResult> JobRequestDetails(int id)
        {
            var model = await _unitOfWork.JobRequests.GetDetailsAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
        public async Task<IActionResult> CreateJobRequest()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            var model = new JobRequestViewModel
            {
                Categories = categories
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJobRequest(JobRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var jobRequest = new JobRequest
                {
                    Title = model.Title,
                    Description = model.Description,
                    Location = model.Location,
                    RightTime = model.RightTime,
                    CategoryId = model.CategoryId,
                    ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                await _unitOfWork.JobRequests.AddAsync(jobRequest);
                await _unitOfWork.SaveAsync();


                if (model.Attachments != null && model.Attachments.Count > 0)
                {

                    var uploadsFolder = Path.Combine("wwwroot", "uploads", "Attachments");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    foreach (var file in model.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using var stream = new FileStream(filePath, FileMode.Create);
                            await file.CopyToAsync(stream);
                            var attachment = new JobRequestAttachment
                            {
                                FilePath = $"/uploads/Attachments/{uniqueFileName}",
                                JobRequestId = jobRequest.Id
                            };

                            await _unitOfWork.Attachments.AddAsync(attachment);
                        }
                    }

                    await _unitOfWork.SaveAsync();
                }

                return RedirectToAction("JobRequestDetails", new { id = jobRequest.Id });
            }

            // Reload categories for the dropdown in case of invalid model state
            model.Categories = await _unitOfWork.Categories.GetAllAsync();
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var jobRequest = await _unitOfWork.JobRequests.GetDetailsAsync(id);
            if (jobRequest == null)
            {
                return NotFound();
            }

            var model = new JobRequestEditViewModel
            {
                Id = jobRequest.Id,
                Title = jobRequest.Title,
                Description = jobRequest.Description,
                Location = jobRequest.Location,
                RightTime = jobRequest.RightTime,
                Status = jobRequest.Status,
                CategoryId = jobRequest.CategoryId,
                Categories = await _unitOfWork.Categories.GetCategoriesAsync()
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobRequestEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _unitOfWork.Categories.GetCategoriesAsync();
                return View(model);
            }

            var jobRequest = await _unitOfWork.JobRequests.GetFirstorDefaultsync(x=>x.Id == model.Id);
            if (jobRequest == null)
            {
                return NotFound();
            }

            jobRequest.Title = model.Title;
            jobRequest.Description = model.Description;
            jobRequest.Location = model.Location;
            jobRequest.RightTime = model.RightTime;
            jobRequest.Status = model.Status;
            jobRequest.CategoryId = model.CategoryId;

            await _unitOfWork.JobRequests.UpdateAsync(jobRequest);

            return RedirectToAction("JobRequestDetails", new { id = jobRequest.Id });
        }
    }
}
