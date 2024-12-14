using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.Entities.ViewModel
{
    public class OfferViewModel
    {
        public int Id { get; set; }
        public int JobRequestId { get; set; }
        public string UserName { get; set; }
        public string ApplicationUserId { get; set; }
        public decimal Price { get; set; }
        public bool status { get; set; }
        public string NegotiationDetails { get; set; }
    }
}
