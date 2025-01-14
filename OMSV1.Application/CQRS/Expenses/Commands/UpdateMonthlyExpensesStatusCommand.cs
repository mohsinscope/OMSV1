using MediatR;

namespace OMSV1.Application.Commands.Expenses
{
    public class UpdateMonthlyExpensesStatusCommand : IRequest<bool>
    {
        public Guid MonthlyExpensesId { get; set; }
        public int NewStatus { get; set; }
        public string Notes { get; set; } = string.Empty; // New property for Notes

        public UpdateMonthlyExpensesStatusCommand(Guid monthlyExpensesId, int newStatus, string notes)
        {
            MonthlyExpensesId = monthlyExpensesId;
            NewStatus = newStatus;
            Notes = notes;
        }
    }
}
