namespace OMSV1.Application.DTOs.Expenses;

public class MonthlyExpensesDto
{
    public Guid Id { get; set; }
    public decimal TotalAmount { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public Guid OfficeId { get; set; }
    public string OfficeName { get; set; } = string.Empty;

    public Guid GovernorateId { get; set; }
    public string GovernorateName { get; set; } = string.Empty;

    public Guid ProfileId { get; set; }
    public string ProfileFullName { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } // Add this property
}
