using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.Entities.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public string NegotiationDetails { get; set; }
        public int Price { get; set; }
        public DateTime Added { get; set; }
        public bool status { get; set; } = false;
        public string ApplicationUserId { get; set; }
        public int JobRequestId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public JobRequest? JobRequest { get; set; }
    }
}
