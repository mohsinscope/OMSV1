using OMSV1.Domain.Entities.Expenses;

namespace OMSV1.Domain.Interfaces
{
    public interface IMonthlyExpensesRepository
    {
        Task<List<MonthlyExpenses>> GetAllMonthlyExpensesAsync();
    }
}
