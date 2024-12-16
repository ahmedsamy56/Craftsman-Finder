using CraftsmanFinder.DataAccess.Data;
using CraftsmanFinder.DataAccess.Repository.IRepository;
using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Entities.ViewModel;
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

        public async Task<IEnumerable<JobRequestListViewModel>> GetByCategoryAsync(int categoryId)
        {
            return await _context.jobRequests
                .Where(jr => jr.CategoryId == categoryId)
                .Select(jr => new JobRequestListViewModel
                {
                    Id = jr.Id,
                    Title = jr.Title,
                    Description = jr.Description,
                    Location = jr.Location,
                    Created = jr.Created,
                    ImagePath = _context.Attachments
                        .Where(a => a.JobRequestId == jr.Id)
                        .Select(a => a.FilePath)
                        .FirstOrDefault()
                })
                .ToListAsync();
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
