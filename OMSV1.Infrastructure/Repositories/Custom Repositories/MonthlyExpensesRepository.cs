using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Interfaces;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Infrastructure.Repositories
{
    public class MonthlyExpensesRepository : IMonthlyExpensesRepository
    {
        private readonly AppDbContext _context;

        public MonthlyExpensesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MonthlyExpenses>> GetAllMonthlyExpensesAsync()
        {
            return await _context.MonthlyExpenses
                .Include(e => e.Office)
                .Include(e => e.Governorate)
                .Include(e => e.Profile)
                .Include(e => e.Threshold)
                .ToListAsync();
        }
        //By Date
public async Task<List<MonthlyExpenses>> GetExpensesByDateRangeAsync(DateTime? startDate = null, DateTime? endDate = null)
{
    var end = endDate?.ToUniversalTime() ?? DateTime.UtcNow;
    var start = startDate?.ToUniversalTime() ?? end.AddDays(-30);

    return await _context.MonthlyExpenses
        .Include(e => e.Profile) // Eagerly load Profile
        .Include(e => e.Office)  // Eagerly load Office
        .Include(e => e.Governorate) // Eagerly load Governorate
        .Where(e => e.DateCreated >= start && e.DateCreated <= end)
        .ToListAsync();
}


    }
}
