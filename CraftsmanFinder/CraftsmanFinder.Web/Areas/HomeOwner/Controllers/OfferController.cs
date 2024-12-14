using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Entities.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CraftsmanFinder.Web.Areas.HomeOwner.Controllers
{
    [Area("HomeOwner")]
    public class OfferController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public OfferController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitOffer(OfferAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
               
                return RedirectToAction("JobRequestDetails", "JobRequest", new { id = model.JobRequestId });
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return RedirectToAction("JobRequestDetails", "JobRequest", new { id = model.JobRequestId });
                }

                var newOffer = new Offer
                {
                    NegotiationDetails = model.Message,
                    Price = model.Amount, 
                    Added = DateTime.Now,
                    status = false, 
                    ApplicationUserId = userId,
                    JobRequestId = model.JobRequestId
                };

                await _unitOfWork.Offers.AddAsync(newOffer);
                await _unitOfWork.SaveAsync();

                
                return RedirectToAction("JobRequestDetails", "JobRequest", new { id = model.JobRequestId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("JobRequestDetails", "JobRequest", new { id = model.JobRequestId });
            }
        }

        public async Task<IActionResult> EditOffer(int id)
        {
            var userId = _userManager.GetUserId(User);
            var offer = await _unitOfWork.Offers.GetFirstorDefaultsync(x=>x.Id == id);

            if (offer == null || offer.ApplicationUserId != userId)
            {
                return Unauthorized();
            }

            var model = new OfferViewModel
            {
                JobRequestId = offer.JobRequestId,
                Price = offer.Price,
                NegotiationDetails = offer.NegotiationDetails,
                ApplicationUserId = offer.ApplicationUserId
            };

            return PartialView("_EditOfferModal", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOffer(OfferEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("JobRequestDetails", "JobRequest", new { id = model.JobRequestId });
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                var existingOffer = await _unitOfWork.Offers.GetFirstorDefaultsync(x=>x.Id == model.OfferId);

                if (existingOffer == null || existingOffer.ApplicationUserId != userId)
                {
                    return Unauthorized();
                }

                existingOffer.Price = (int)model.Price;
                existingOffer.NegotiationDetails = model.NegotiationDetails;

                await _unitOfWork.Offers.UpdateAsync(existingOffer );
                await _unitOfWork.SaveAsync();

                
                return RedirectToAction("JobRequestDetails", "JobRequest", new { id = model.JobRequestId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("JobRequestDetails", "JobRequest", new { id = model.JobRequestId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOffer(int offerId)
        {
            int JobRequestId = 0;
            try
            {
                var userId = _userManager.GetUserId(User);
                var offer = await _unitOfWork.Offers.GetFirstorDefaultsync(x=>x.Id == offerId);

                if (offer == null || offer.ApplicationUserId != userId)
                {
                    return Unauthorized();
                }
                JobRequestId = offer.JobRequestId;
                await _unitOfWork.Offers.DeleteAsync(offer);
                await _unitOfWork.SaveAsync();


                return RedirectToAction("JobRequestDetails", "JobRequest", new { id = JobRequestId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("JobRequestDetails", "JobRequest", new { id = JobRequestId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptOffer(int offerId)
        {
            int JobRequestId = 0;   
            try
            {
                // Validate user permissions
                var offer = await _unitOfWork.Offers.GetFirstorDefaultsync(x=>x.Id == offerId);
                JobRequestId = offer.JobRequestId;
                if (offer == null)
                {
                    return RedirectToAction("JobRequestDetails", "JobRequest", new { id = JobRequestId });
                }

                var userId = _userManager.GetUserId(User);
                var jobRequest = await _unitOfWork.JobRequests.GetFirstorDefaultsync(x=>x.Id == JobRequestId);

                if (jobRequest == null || jobRequest.ApplicationUserId != userId)
                {
                    return Unauthorized();
                }

                // Accept the offer
                await _unitOfWork.Offers.AcceptOfferAsync(offerId);

                return RedirectToAction("JobRequestDetails", "JobRequest", new { id = JobRequestId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("JobRequestDetails", "JobRequest", new { id = JobRequestId });
            }
        }

    }
}
