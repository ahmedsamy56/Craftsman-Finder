using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ProfileImagePath { get; set; }

        //Craftsman Property
        public string? AboutMe { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<Certificate>? certificates { get; set; }
        public ICollection<JobRequest>? JobRequests { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
