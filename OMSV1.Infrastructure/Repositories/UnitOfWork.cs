using System;
using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
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
