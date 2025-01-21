using OMSV1.Domain.Entities.Expenses;

namespace OMSV1.Infrastructure.Interfaces
{
 public interface IPdfService
{
    // Task<string> GenerateAsync(string title, string content, Dictionary<string, string>? metadata = null);
    Task<string> GenerateMonthlyExpensesPdfAsync(List<MonthlyExpenses> expenses);
}

}
