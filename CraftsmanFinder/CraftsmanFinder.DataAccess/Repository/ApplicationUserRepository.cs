using CraftsmanFinder.DataAccess.Data;
using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Entities.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser> , IApplicationUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserRepository(AppDbContext context, UserManager<ApplicationUser> userManager) : base(context) 
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            if (role == null)
                return Enumerable.Empty<ApplicationUser>();

            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

            return usersInRole;
        }
        public async Task<IEnumerable<ApplicationUser>> GetUsersByCategoryAsync(int categoryId)
        {
            return await _context.users
                .Where(user => user.CategoryId == categoryId) 
                .Include(user => user.Category)
                .ToListAsync();
        }

        public async Task<HomeOwnerProfileViewModel> GetHomeOwnerProfileAsync(string userId)
        {
            var user = await _context.users
                .Include(u => u.JobRequests)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return new HomeOwnerProfileViewModel
            {
                Id = userId,
                Name = user.Name,
                Address = user.Address,
                PhoneNumebr = user.PhoneNumber,
                ProfileImagePath = user.ProfileImagePath,
                Email = user.Email,
                JobRequests = user.JobRequests
            };
        }


        public async Task<ApplicationUser> GetUserWithAllDetailsAsync(string userId)
        {
            return await _context.users
            .Include(user => user.Reviews)
            .Include(user => user.certificates)
            .Include(user => user.Category)
            .FirstOrDefaultAsync(user => user.Id == userId);

        }
    }
}
