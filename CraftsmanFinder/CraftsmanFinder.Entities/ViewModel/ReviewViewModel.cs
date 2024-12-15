using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.Entities.ViewModel
{
    public class ReviewViewModel
    {
        public double Rating { get; set; }
        public string Comment { get; set; }
        public int JobRequestId { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
