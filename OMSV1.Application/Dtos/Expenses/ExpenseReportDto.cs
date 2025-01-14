namespace OMSV1.Application.DTOs.Expenses
{
    public class ExpenseReportDto
    {
        public int TotalCount { get; set; }
        public decimal TotalAmount { get; set; }
        public double TotalPercentage { get; set; }
        public List<MonthlyExpenseDetailDto> Expenses { get; set; }

    }
}