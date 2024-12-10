using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OMSV1.Domain.Entities.Profiles;

namespace OMSV1.Domain.SeedWork
{
    public interface IGenericRepository<T> where T : Entity
    {

        Task<T> AddAsync(T entity);
        void DeleteAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task UpdateAsync(T entity);
    }
}

