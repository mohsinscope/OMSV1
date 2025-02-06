using MediatR;
using System;
using System.Collections.Generic;

namespace OMSV1.Application.Commands.Expenses
{
    public class UpdateDailyExpensesCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid ExpenseTypeId { get; set; }

        // Complete list of subexpense updates (existing ones and/or new ones)
        public List<UpdateSubExpenseItem>? SubExpenseUpdates { get; set; }

        public UpdateDailyExpensesCommand(
            Guid id,
            decimal price,
            int quantity,
            string notes,
            DateTime expenseDate,
            Guid expenseTypeId,
            List<UpdateSubExpenseItem>? subExpenseUpdates = null)
        {
            Id = id;
            Price = price;
            Quantity = quantity;
            Notes = notes;
            ExpenseDate = expenseDate;
            ExpenseTypeId = expenseTypeId;
            SubExpenseUpdates = subExpenseUpdates;
        }
    }

    public class UpdateSubExpenseItem
    {
        /// <summary>
        /// If updating an existing subexpense, this is its Id.
        /// If null or Guid.Empty, this represents a new subexpense.
        /// </summary>
        public Guid? Id { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime? ExpenseDate { get; set; }
        public Guid ExpenseTypeId { get; set; }
    }
}
