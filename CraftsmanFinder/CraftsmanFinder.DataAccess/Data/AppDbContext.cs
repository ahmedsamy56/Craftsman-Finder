
using CraftsmanFinder.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Certificate> certificates { get; set; }
        public DbSet<JobRequest> jobRequests { get; set; }
        public DbSet<JobRequestAttachment> Attachments { get; set; }
        public DbSet<Offer> offers { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<Notification> notifications { get; set; }

    }
}
