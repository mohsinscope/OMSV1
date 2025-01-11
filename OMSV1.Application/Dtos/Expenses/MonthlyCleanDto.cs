namespace OMSV1.Application.DTOs.Expenses;

public class MonthlyCleanDto
{
    public decimal TotalAmount { get; set; }
    public string OfficeName { get; set; } = string.Empty;
    public string GovernorateName { get; set; } = string.Empty;
    public string ThresholdName { get; set; } = string.Empty;
    public decimal PercentageOfBudget { get; set; } // New field
 // Only show the threshold name

}
