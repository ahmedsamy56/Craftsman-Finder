using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        [ValidateNever]
        public string Logo { get; set; }
        [ValidateNever]
        public ICollection<ApplicationUser>? ApplicationUsers { get; set; }
        [ValidateNever]
        public ICollection<JobRequest>? JobRequests { get; set; }
    }
}
