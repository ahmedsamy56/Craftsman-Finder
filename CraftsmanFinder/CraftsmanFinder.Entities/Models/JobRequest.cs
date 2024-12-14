using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.Entities.Models
{
    public class JobRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime RightTime { get; set; }
        public bool Status { get; set; } = false;
        public int CategoryId { get; set; }
        public string ApplicationUserId { get; set; }
        public Category? Category { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<JobRequestAttachment>? Attachment { get; set; }
        public ICollection<Offer>? offers { get; set; }

    }
}
