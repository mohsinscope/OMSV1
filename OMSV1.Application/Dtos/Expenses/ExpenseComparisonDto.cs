namespace OMSV1.Application.DTOs.Expenses;

public class ExpenseComparisonDto
{
    public decimal TotalAmount { get; set; }
    public string OfficeName { get; set; } = string.Empty;
    public string GovernorateName { get; set; } = string.Empty;
    public string ThresholdName { get; set; } = string.Empty;
    public decimal PercentageOfBudget { get; set; } // New field
    public bool IsCurrentMonth { get; set; }
    public bool IsLastMonth { get; set; }
 // Only show the threshold name

}
