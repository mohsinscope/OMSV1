using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace OMSV1.Infrastructure.Repositories
{
    public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : Entity
    {
        private readonly AppDbContext _context = context;

        public async Task<T> AddAsync(T entity)
        {
            var addedEntity = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync(); // Persist deletion to the database
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id); // Lazy loading
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        // Eager loading method with optional includes
       public async Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes)
            {
                IQueryable<T> query = _context.Set<T>();

                // Apply includes for eager loading
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return await query.FirstOrDefaultAsync(e => e.Id == id);
            }
            
    }
}
