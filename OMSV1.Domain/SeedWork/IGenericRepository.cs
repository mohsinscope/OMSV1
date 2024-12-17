using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Specifications;

namespace OMSV1.Domain.SeedWork
{
    public interface IGenericRepository<T> where T : Entity
    {


        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        IQueryable<T> GetAllAsQueryable();
        Task<T> SingleOrDefaultAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        Task<T> AddAsync(T entity);
          // Add this method for first or default functionality
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task DeleteAsync(T entity); // Return Task, not void
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task UpdateAsync(T entity);
        Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
    }
}
