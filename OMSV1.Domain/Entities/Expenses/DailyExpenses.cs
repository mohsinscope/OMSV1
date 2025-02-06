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






// public class DailyExpenses : Entity
// {
//     public decimal Price { get; private set; }
//     public int Quantity { get; private set; }
//     public decimal Amount { get; private set; }
//     public string Notes { get; private set; }
//     public DateTime ExpenseDate { get; private set; }
//     public Guid ExpenseTypeId { get; private set; }
//     public Guid MonthlyExpensesId { get; private set; }

//     public ExpenseType? ExpenseType { get; private set; }
//     public MonthlyExpenses? MonthlyExpenses { get; private set; }

//     // Self-reference: Parent and children
//     public Guid? ParentExpenseId { get; private set; }
//     public DailyExpenses? ParentExpense { get; private set; }
//     private readonly List<DailyExpenses> _subExpenses = new();
//     public IReadOnlyCollection<DailyExpenses> SubExpenses => _subExpenses.AsReadOnly();

//     // Constructor for main daily expense (parent)
//     public DailyExpenses(decimal price, int quantity, string notes, DateTime expenseDate, Guid expenseTypeId, Guid monthlyExpensesId)
//     {
//         Price = price;
//         Quantity = quantity;
//         Amount = price * quantity;
//         Notes = notes;
//         ExpenseDate = DateTime.SpecifyKind(expenseDate, DateTimeKind.Utc);
//         ExpenseTypeId = expenseTypeId;
//         MonthlyExpensesId = monthlyExpensesId;
//     }

//     // Constructor for sub-expenses (child)
//     public DailyExpenses(decimal price, int quantity, string notes, Guid parentExpenseId)
//     {
//         Price = price;
//         Quantity = quantity;
//         Amount = price * quantity;
//         Notes = notes;
//         ExpenseDate = DateTime.UtcNow;
//         ParentExpenseId = parentExpenseId;
//     }

//     // Add a sub-item (child expense)
//     public void AddSubExpense(decimal price, int quantity, string notes)
//     {
//         var subExpense = new DailyExpenses(price, quantity, notes, this.Id);
//         _subExpenses.Add(subExpense);
//     }

//     // Update existing expense or sub-expense
//     public void Update(decimal price, int quantity, string notes)
//     {
//         Price = price;
//         Quantity = quantity;
//         Amount = price * quantity;
//         Notes = notes;
//     }

//     // Calculate total amount including sub-items
//     public decimal GetTotalAmount()
//     {
//         return _subExpenses.Any() ? _subExpenses.Sum(e => e.Amount) : Amount;
//     }
// }

