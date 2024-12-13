using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.Entities.Models
{
    public class JobRequestAttachment
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public int JobRequestId { get; set; }
        public JobRequest JobRequest { get; set; }
    }
}
