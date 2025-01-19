using MediatR;
using System;

namespace OMSV1.Application.Commands.Expenses
{
    public class AddActionCommand : IRequest<Guid>
    {
        public string ActionType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public Guid MonthlyExpensesId { get; set; }

        public AddActionCommand(string actionType, string notes,Guid monthlyExpensesId)
        {
            ActionType = actionType;
            Notes = notes;
            MonthlyExpensesId = monthlyExpensesId;
        }
    }
}
