using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications;
using OMSV1.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace OMSV1.Infrastructure.Repositories
{
    public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : Entity
    {
    private readonly AppDbContext _context = context;
    private readonly DbSet<T> _dbSet = context.Set<T>(); 


        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbSet.ToListAsync();
        }


        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }


        public IQueryable<T> ListAsQueryable(ISpecification<T> spec)
            {
                return ApplySpecification(spec);
            }
            
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)

            {
                return await _context.Set<T>().AnyAsync(predicate);
            }


        public IQueryable<T> GetAllAsQueryable()
            {
                return _dbSet.AsNoTracking(); // Returns the data as IQueryable for further processing
            }


        public async Task<T> SingleOrDefaultAsync(ISpecification<T> spec)
            {
                return await ApplySpecification(spec).FirstOrDefaultAsync();
            }
   

        public async Task<int> CountAsync(ISpecification<T> spec)
            {
                return await ApplySpecification(spec).CountAsync();
            }

        
        public async Task<T> AddAsync(T entity)
            {
                var addedEntity = await _context.Set<T>().AddAsync(entity);
                
                return addedEntity.Entity;
            }

        public async Task DeleteAsync(T entity)
            {
                _context.Set<T>().Remove(entity);
            
            }

        public async Task<T?> GetByIdAsync(Guid id)
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
                
            }
        //Profile by Id
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
            {
                return await _context.Set<T>().FirstOrDefaultAsync(predicate);
            }
             public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }


        // Eager loading method with optional includes
       public async Task<T?> GetByIdWithIncludesAsync(Guid id, params Expression<Func<T, object>>[] includes)
            {
                IQueryable<T> query = _context.Set<T>();

                // Apply includes for eager loading
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return await query.FirstOrDefaultAsync(e => e.Id == id);
            }

        // Implement AnyAsync method
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().AnyAsync(predicate, cancellationToken);
        }

                private IQueryable<T> ApplySpecification(ISpecification<T> spec)
                    {
                        var query = _context.Set<T>().AsQueryable();

                        if (spec.Criteria != null)
                        {
                            query = query.Where(spec.Criteria);
                        }

                        if (spec.OrderBy != null)
                        {
                            query = query.OrderBy(spec.OrderBy);
                        }

                        if (spec.OrderByDescending != null)
                        {
                            query = query.OrderByDescending(spec.OrderByDescending);
                        }

                        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

                        return query;
                    }


                // private IQueryable<T> ApplySpecification(ISpecification<T> spec)
                //     {
                //         var query = _context.Set<T>().AsQueryable();

                //         if (spec.Criteria != null)
                //             query = query.Where(spec.Criteria);

                //         if (spec.Includes != null)
                //             query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

                //         return query;
                //     }

           
            //sorting
       public IQueryable<T> ApplySorting(IQueryable<T> query, ISpecification<T> spec)
        {
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.SecondaryOrderByDescending != null)
            {
                query = ((IOrderedQueryable<T>)query).ThenByDescending(spec.SecondaryOrderByDescending);
            }

            return query;
        }


    }

}





