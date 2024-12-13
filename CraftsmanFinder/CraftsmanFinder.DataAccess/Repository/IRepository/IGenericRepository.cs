using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetFirstorDefaultsync(Expression<Func<T, bool>> perdicate = null, string? Includeword = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> perdicate = null, string? Includeword = null);
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
