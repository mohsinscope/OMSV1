using System;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Expenses;

public class DailyExpenses(decimal price, int quantity, decimal amount, string notes, DateTime expenseDate, Guid expenseTypeId, Guid monthlyExpensesId) : Entity
{
    public decimal Price { get; private set; } = price;
    public int Quantity { get; private set; } = quantity;
    public decimal Amount { get; private set; } = amount;
    public string Notes { get; private set; } = notes;
    public DateTime ExpenseDate { get; private set; } = DateTime.SpecifyKind(expenseDate, DateTimeKind.Utc); // Ensure UTC
    public Guid ExpenseTypeId { get; private set; } = expenseTypeId;
    public Guid MonthlyExpensesId { get; private set; } = monthlyExpensesId;
    public ExpenseType? ExpenseType { get; private set; }
    public MonthlyExpenses? MonthlyExpenses { get; private set; }

    public void Update(decimal price, int quantity, decimal amount, string notes, DateTime expenseDate, Guid expenseTypeId)
    {
        Price = price;
        Quantity = quantity;
        Amount = amount;
        Notes = notes;
        ExpenseDate = DateTime.SpecifyKind(expenseDate, DateTimeKind.Utc); // Ensure UTC
        ExpenseTypeId = expenseTypeId;
    }

}
