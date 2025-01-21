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
    }
}
