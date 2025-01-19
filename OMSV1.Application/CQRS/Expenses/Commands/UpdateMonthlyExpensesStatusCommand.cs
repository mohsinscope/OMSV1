using MediatR;

namespace OMSV1.Application.Commands.Expenses
{
    public class UpdateMonthlyExpensesStatusCommand : IRequest<bool>
    {
        public Guid MonthlyExpensesId { get; set; }
        public int NewStatus { get; set; }

        public UpdateMonthlyExpensesStatusCommand(Guid monthlyExpensesId, int newStatus)
        {
            MonthlyExpensesId = monthlyExpensesId;
            NewStatus = newStatus;
        }
    }
}
