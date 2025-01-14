    namespace OMSV1.Application.DTOs.Expenses;
    public class MonthlyExpenseDetailDto
    {
        public decimal TotalAmount { get; set; }
        public string OfficeName { get; set; }
        public string GovernorateName { get; set; }
        public double PercentageOfBudget { get; set; }
        public DateTime DateCreated { get; set; }
    }