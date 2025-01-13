using MediatR;
using OMSV1.Application.DTOs.Expenses;

namespace OMSV1.Application.Queries.Expenses
{
    public class GetDailyExpenseByIdQuery : IRequest<DailyExpensesDto?>
    {
        public Guid Id { get; set; } // The ID of the DailyExpense to retrieve
        public GetDailyExpenseByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
