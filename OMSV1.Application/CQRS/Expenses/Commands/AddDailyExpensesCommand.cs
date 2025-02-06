using MediatR;
using System;
using System.Collections.Generic;

namespace OMSV1.Application.Commands.Expenses
{
    public class AddDailyExpensesCommand : IRequest<Guid>
    {
        // Parent Expense properties
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime ExpenseDate { get; set; }
        public Guid ExpenseTypeId { get; set; }
        public Guid MonthlyExpensesId { get; set; }

        // Optional list of subexpense items.
        public List<SubExpenseItem>? SubExpenses { get; set; }

        /// <summary>
        /// Constructor for creating a main (parent) daily expense.
        /// </summary>
        public AddDailyExpensesCommand(decimal price, int quantity, string notes, DateTime expenseDate, Guid expenseTypeId, Guid monthlyExpensesId)
        {
            Price = price;
            Quantity = quantity;
            Notes = notes;
            ExpenseDate = expenseDate;
            ExpenseTypeId = expenseTypeId;
            MonthlyExpensesId = monthlyExpensesId;
        }

        // Parameterless constructor to allow model binding for the subexpenses collection
        public AddDailyExpensesCommand() { }
    }

    // Updated DTO representing a subexpense item.
    public class SubExpenseItem
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; } = string.Empty;
        // New property so that each subexpense can have its own ExpenseTypeId.
        public Guid ExpenseTypeId { get; set; }
    }
}
