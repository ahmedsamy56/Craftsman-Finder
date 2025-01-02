using System;
using CraftsmanFinder.DataAccess.Data;
using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Web.Mvc;
namespace CraftsmanFinder.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services
             .AddIdentity<ApplicationUser, IdentityRole>()
             .AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders()
             .AddDefaultUI();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddSingleton<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.MapRazorPages();

            app.MapStaticAssets();
            app.MapControllers();
            app.MapControllerRoute(
              name: "HomeOwner",
              pattern: "{area:exists}/{controller=Home}/{action=Landing}/{id?}",
              defaults: new { area = "HomeOwner" });
            app.MapControllerRoute(
               name: "Crafts",
               pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}",
               defaults: new { area = "Crafts" });
            app.MapControllerRoute(
                name: "Admin",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}",
                defaults: new { area = "Admin" });
         
            app.Run();
        }
    }
}
