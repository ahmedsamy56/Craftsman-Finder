﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.Entities.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string  Content { get; set; }
        public string Sender { get; set; }
        public bool NeedAction { get; set; }
        public string? Link { get; set; }
        public bool IsWatched { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
