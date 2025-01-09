namespace OMSV1.Application.DTOs.Expenses;

public class DailyExpensesDto
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Amount { get; set; }
    public string Notes { get; set; }
    public DateTime ExpenseDate { get; set; }
    public Guid ExpenseTypeId { get; set; }
}
