using MediatR;
using OMSV1.Application.DTOs.Expenses;

namespace OMSV1.Application.Queries.Actions
{
    public class GetActionsByMonthlyExpensesIdQuery : IRequest<List<ActionDto>>
    {
        public Guid MonthlyExpensesId { get; set; }

        public GetActionsByMonthlyExpensesIdQuery(Guid monthlyExpensesId)
        {
            MonthlyExpensesId = monthlyExpensesId;
        }
    }
}
