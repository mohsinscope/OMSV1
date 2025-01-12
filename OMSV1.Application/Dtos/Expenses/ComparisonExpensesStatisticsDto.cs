 using System.Collections.Generic;

namespace OMSV1.Application.DTOs.Expenses;
 public class ComparisonExpensesStatisticsDto
    {
        public ExpensesStatisticsDto ThisMonth { get; set; }
        public ExpensesStatisticsDto LastMonth { get; set; }
    }
