using System;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Expenses;

public class DailyExpenses : Entity
{
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    public decimal Amount { get; private set; }
    public string Notes { get; private set; }
    public DateTime ExpenseDate { get; private set; }
    public Guid ExpenseTypeId { get; private set; }
    public Guid MonthlyExpensesId { get; private set; }
    public ExpenseType ExpenseType { get; private set; }
    public MonthlyExpenses MonthlyExpenses { get; private set; }

    public DailyExpenses(decimal price, int quantity, decimal amount, string notes, DateTime expenseDate, Guid expenseTypeId, Guid monthlyExpensesId)
    {
        Price = price;
        Quantity = quantity;
        Amount = amount;
        Notes = notes;
        ExpenseDate = DateTime.SpecifyKind(expenseDate, DateTimeKind.Utc); // Ensure UTC
        ExpenseTypeId = expenseTypeId;
        MonthlyExpensesId = monthlyExpensesId;
    }
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
