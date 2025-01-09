namespace OMSV1.Application.DTOs.Expenses;

public class DailyExpensesResponseDto
{
    public Guid MonthlyExpensesId { get; set; }
    public List<DailyExpensesDto> DailyExpenses { get; set; } = new List<DailyExpensesDto>();
}
