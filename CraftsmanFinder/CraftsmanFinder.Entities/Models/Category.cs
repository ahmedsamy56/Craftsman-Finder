using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.Entities.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public ICollection<ApplicationUser>? ApplicationUsers { get; set; }
        public ICollection<JobRequest>? JobRequests { get; set; }
    }
}
