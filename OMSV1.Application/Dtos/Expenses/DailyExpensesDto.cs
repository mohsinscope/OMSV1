using OMSV1.Domain.Entities.Expenses;

namespace OMSV1.Application.DTOs.Expenses
{
    public class DailyExpensesDto
    {
        public Guid MonthlyExpensesId { get; set; }
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid ExpenseTypeId { get; set; }
        public string? ExpenseTypeName { get; set; } // Include only ExpenseType name
        
        // Optional: The parent expense ID (null if this is a top-level expense)
        public Guid? ParentExpenseId { get; set; }
        
        // Optional: A list of subexpense DTOs (empty or null if there are none)
        public List<DailyExpensesDto>? SubExpenses { get; set; }

        // Calculate the total amount (including sub-expenses)
        public decimal TotalAmount
        {
            get
            {
                decimal totalAmount = Amount;
                if (SubExpenses != null && SubExpenses.Any())
                {
                    totalAmount += SubExpenses.Sum(sub => sub.Amount);
                }
                return totalAmount;
            }
        }
    }
}
