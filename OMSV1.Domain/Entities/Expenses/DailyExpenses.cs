using System;
using System.Collections.Generic;
using System.Linq;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Expenses
{
    public class DailyExpenses : Entity
    {
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public decimal Amount { get; private set; }
        public string Notes { get; private set; }
        public DateTime ExpenseDate { get; private set; }
        public Guid ExpenseTypeId { get; private set; }
        public Guid MonthlyExpensesId { get; private set; }

        public ExpenseType? ExpenseType { get; private set; }
        public MonthlyExpenses? MonthlyExpenses { get; private set; }

        // Self-reference: Parent and children
        public Guid? ParentExpenseId { get; private set; }
        public DailyExpenses? ParentExpense { get; private set; }
        private readonly List<DailyExpenses> _subExpenses = new();
        public IReadOnlyCollection<DailyExpenses> SubExpenses => _subExpenses.AsReadOnly();

        // Constructor for main daily expense (parent)
        public DailyExpenses(decimal price, int quantity, string notes, DateTime expenseDate, Guid expenseTypeId, Guid monthlyExpensesId)
        {
            Price = price;
            Quantity = quantity;
            Amount = price * quantity;
            Notes = notes;
            // Ensure ExpenseDate is stored as UTC
            ExpenseDate = DateTime.SpecifyKind(expenseDate, DateTimeKind.Utc);
            ExpenseTypeId = expenseTypeId;
            MonthlyExpensesId = monthlyExpensesId;
        }

        // Updated constructor for sub-expenses (child) that now also requires monthlyExpensesId.
        public DailyExpenses(decimal price, int quantity, string notes, Guid parentExpenseId, Guid expenseTypeId, Guid monthlyExpensesId)
        {
            Price = price;
            Quantity = quantity;
            Amount = price * quantity;
            Notes = notes;
            // Use current UTC time for subexpenses
            ExpenseDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            ParentExpenseId = parentExpenseId;
            ExpenseTypeId = expenseTypeId;
            MonthlyExpensesId = monthlyExpensesId;
        }

        /// <summary>
        /// Adds a subexpense to this DailyExpenses.
        /// </summary>
        /// <param name="price">Subexpense price.</param>
        /// <param name="quantity">Subexpense quantity.</param>
        /// <param name="notes">Subexpense notes.</param>
        /// <param name="expenseTypeId">Subexpense ExpenseTypeId (can be different from the parent's).</param>
        public void AddSubExpense(decimal price, int quantity, string notes, Guid expenseTypeId)
        {
            // Pass the parent's MonthlyExpensesId to the subexpense.
            var subExpense = new DailyExpenses(price, quantity, notes, this.Id, expenseTypeId, this.MonthlyExpensesId);
            _subExpenses.Add(subExpense);
        }

        // // Update existing expense or sub-expense
        // public void Update(decimal price, int quantity, string notes)
        // {
        //     Price = price;
        //     Quantity = quantity;
        //     Amount = price * quantity;
        //     Notes = notes;
        // }
        public void Update(decimal price, int quantity, string notes, DateTime expenseDate, Guid expenseTypeId)
        {
            Price = price;
            Quantity = quantity;
            Amount = price * quantity;
            Notes = notes;
            ExpenseDate = DateTime.SpecifyKind(expenseDate, DateTimeKind.Utc);
            ExpenseTypeId = expenseTypeId;
        }

        // Calculate total amount including sub-items.
        // If sub-expenses exist, it returns their sum; otherwise, the expense's own Amount.
        public decimal GetTotalAmount()
        {
            return _subExpenses.Any() ? _subExpenses.Sum(e => e.Amount) : Amount;
        }
    }
}
