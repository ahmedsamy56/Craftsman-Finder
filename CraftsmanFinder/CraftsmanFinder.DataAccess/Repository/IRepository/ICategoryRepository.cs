﻿using CraftsmanFinder.Entities.Models;
using CraftsmanFinder.Entities.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task UpdateAsync(Category category);
        public  Task<IEnumerable<SelectListItem>> GetCategoriesAsync();
        public Task<IEnumerable<JobRequestListViewModel>> GetByCategoryAsync(int categoryId);
    }
}
