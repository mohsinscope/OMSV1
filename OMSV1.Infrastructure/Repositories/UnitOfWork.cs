using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : Entity
    {
        if (!_repositories.ContainsKey(typeof(TEntity)))
        {
            var repositoryInstance = new GenericRepository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), repositoryInstance);
        }
        return (IGenericRepository<TEntity>)_repositories[typeof(TEntity)];
    }

    public async Task<bool> SaveAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"An error occurred while saving changes: {ex.Message}");
            return false;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
