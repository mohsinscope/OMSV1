using MediatR;

namespace OMSV1.Application.Commands.Expenses;

public class UpdateDailyExpensesCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Amount { get; set; }
    public string Notes { get; set; }
    public DateTime ExpenseDate { get; set; }
    public Guid ExpenseTypeId { get; set; }

    public UpdateDailyExpensesCommand(Guid id, decimal price, int quantity, string notes, DateTime expenseDate, Guid expenseTypeId)
    {
        Id = id;
        Price = price;
        Quantity = quantity;
        Amount = price * quantity; // Calculate Amount
        Notes = notes;
        ExpenseDate = expenseDate;
        ExpenseTypeId = expenseTypeId;
    }
}
