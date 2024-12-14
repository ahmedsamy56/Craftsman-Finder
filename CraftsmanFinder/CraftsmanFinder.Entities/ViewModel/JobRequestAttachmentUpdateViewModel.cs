using CraftsmanFinder.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.Entities.ViewModel
{
    public class JobRequestAttachmentUpdateViewModel
    {
        public int JobRequestId { get; set; }
        public List<JobRequestAttachment> ExistingAttachments { get; set; }
        public List<IFormFile> NewAttachments { get; set; }
    }
}
