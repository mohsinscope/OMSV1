using OMSV1.Domain.Entities.Expenses;

namespace OMSV1.Application.DTOs.Expenses;

public class DailyExpensesDto
{
    public Guid MonthlyExpensesId { get; set; }
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Amount { get; set; }
    public string Notes { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string ExpenseTypeName { get; set; } // Include only ExpenseType name
}
