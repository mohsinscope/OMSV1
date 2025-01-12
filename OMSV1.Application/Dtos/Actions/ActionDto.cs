namespace OMSV1.Application.DTOs.Expenses
{
    public class ActionDto
    {
        public Guid Id { get; set; }
        public string ActionType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public Guid ProfileId { get; set; }
        public string ProfileFullName { get; set; } = string.Empty; // Assuming Profile has a Name
        public Guid MonthlyExpensesId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
