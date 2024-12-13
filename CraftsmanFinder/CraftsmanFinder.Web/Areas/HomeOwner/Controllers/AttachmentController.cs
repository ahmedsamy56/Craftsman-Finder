using CraftsmanFinder.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace CraftsmanFinder.Web.Areas.HomeOwner.Controllers
{
    public class AttachmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttachmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

      
    }
}
