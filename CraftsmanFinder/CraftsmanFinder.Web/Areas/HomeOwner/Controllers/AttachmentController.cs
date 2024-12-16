using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Entities.ViewModel;
using CraftsmanFinder.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftsmanFinder.Web.Areas.HomeOwner.Controllers
{
    [Area("HomeOwner")]
    [Authorize(Roles = $"{SD.HomeOwnerRole}")]
    public class AttachmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttachmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAttachments(int id)
        {
            var jobRequest = await _unitOfWork.JobRequests.GetFirstorDefaultsync(x=>x.Id == id);
            if (jobRequest == null)
            {
                return View("/Views/Shared/NotFound.cshtml");
            }

            var existingAttachments = await _unitOfWork.Attachments.GetAttachmentsByJobRequestIdAsync(id);

            var viewModel = new JobRequestAttachmentUpdateViewModel
            {
                JobRequestId = id,
                ExistingAttachments = existingAttachments.ToList()
            };

            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> UpdateAttachments(int jobRequestId, List<int> attachmentsToRemove, List<IFormFile> newAttachments)
        {
            if (attachmentsToRemove != null && attachmentsToRemove.Count > 0)
            {
                foreach (var attachmentId in attachmentsToRemove)
                {
                    var attachment = await _unitOfWork.Attachments.GetFirstorDefaultsync(x => x.Id == attachmentId);
                    if (attachment != null && attachment.JobRequestId == jobRequestId)
                    {
                        var filePath = Path.Combine("wwwroot", attachment.FilePath.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath); 
                        }

                        await _unitOfWork.Attachments.DeleteAsync(attachment); 
                    }
                }
            }

            if (newAttachments != null && newAttachments.Count > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot", "uploads", "Attachments");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in newAttachments)
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
                            JobRequestId = jobRequestId
                        };

                        await _unitOfWork.Attachments.AddAsync(attachment);
                    }
                }
            }

            await _unitOfWork.SaveAsync();

            return RedirectToAction("JobRequestDetails", "JobRequest", new { id = jobRequestId });
        }



    }
}
