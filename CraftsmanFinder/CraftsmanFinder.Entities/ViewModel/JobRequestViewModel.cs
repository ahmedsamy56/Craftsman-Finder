using CraftsmanFinder.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.Entities.ViewModel
{

    public class JobRequestViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime RightTime { get; set; }

        [Required]
        public int CategoryId { get; set; } 

        [ValidateNever]
        public IEnumerable<Category> Categories { get; set; }

        public List<IFormFile> Attachments { get; set; } 
    }
}
