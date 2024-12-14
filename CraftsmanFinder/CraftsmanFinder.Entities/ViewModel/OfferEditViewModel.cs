using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.Entities.ViewModel
{
    public class OfferEditViewModel
    {
        public int OfferId { get; set; }
        public int Price { get; set; }
        public string NegotiationDetails { get; set; }
        public int JobRequestId { get; set; }
    }
}
