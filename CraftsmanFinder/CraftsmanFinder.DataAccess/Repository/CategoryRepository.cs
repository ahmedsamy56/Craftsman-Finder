using CraftsmanFinder.DataAccess.Data;
using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetCategoriesAsync()
        {
            var categories = await _context.categories.ToListAsync();

            return categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
        }
        public async  Task  UpdateAsync(Category category)
        {
            var objFromDb = await _context.categories.FirstOrDefaultAsync(c => c.Id == category.Id);

            if (objFromDb == null)
            {
                throw new Exception("Category not found.");
            }
            objFromDb.Name = category.Name;
            objFromDb.Description = category.Description;
            objFromDb.Logo = category.Logo;
            await _context.SaveChangesAsync();
        }
    }
}
