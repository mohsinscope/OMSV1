using MediatR;
using OMSV1.Application.DTOs.Expenses;

namespace OMSV1.Application.Queries.Expenses
{
    public class GetMonthlyExpensesByIdQuery : IRequest<MonthlyExpensesDto>
    {
        public Guid Id { get; set; }

        public GetMonthlyExpensesByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
