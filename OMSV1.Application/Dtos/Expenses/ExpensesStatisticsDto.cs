using System.Collections.Generic;

namespace OMSV1.Application.DTOs.Expenses
{
    public class ExpensesStatisticsDto
    {
        public int TotalCount { get; set; } // Total number of records
        public decimal TotalAmount { get; set; } // Sum of all TotalAmounts
        public List<MonthlyCleanDto> Expenses { get; set; } = new List<MonthlyCleanDto>(); // Paginated results
    }
}
