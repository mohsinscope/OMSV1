using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

using OMSV1.Domain.Entities.Governorates;

namespace OMSV1.Domain.SeedWork
{
    public interface IGenericRepository<T> where T : Entity
    {
        object Profiles { get; }

        Task<T> AddAsync(T entity);
        Task DeleteAsync(T entity); // Return Task, not void
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task UpdateAsync(T entity);
    Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}

