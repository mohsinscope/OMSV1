using System;

namespace OMSV1.Domain.SeedWork;

public interface IUnitOfWork : IDisposable
{
    
    Task<bool> SaveAsync(CancellationToken cancellationToken= default);
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : Entity;
}
