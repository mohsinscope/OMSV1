using MediatR;

namespace OMSV1.Application.Commands.Expenses;

public class AddDailyExpensesCommand : IRequest<Guid>
{
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime ExpenseDate { get; set; }
    public Guid ExpenseTypeId { get; set; }
    public Guid MonthlyExpensesId { get; set; }

    public AddDailyExpensesCommand(decimal price, int quantity, string notes, DateTime expenseDate, Guid expenseTypeId, Guid monthlyExpensesId)
    {
        Price = price;
        Quantity = quantity;
        Notes = notes;
        ExpenseDate = expenseDate;
        ExpenseTypeId = expenseTypeId;
        MonthlyExpensesId = monthlyExpensesId;
    }
}
