using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.Entities.ViewModel
{
    public class JobRequestDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime RightTime { get; set; }
        public bool Status { get; set; }
        public string ApplicationUserId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public List<string> Attachments { get; set; }
        public List<OfferViewModel> Offers { get; set; }
    }
}
