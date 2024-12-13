using CraftsmanFinder.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.Entities.ViewModel
{
    public class HomeOwnerProfileViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumebr { get; set; }
        public string ProfileImagePath { get; set; } 
        public string Email { get; set; }
        public ICollection<JobRequest>? JobRequests { get; set; }
    }
}
