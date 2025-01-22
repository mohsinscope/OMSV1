using MediatR;

namespace OMSV1.Application.Queries.Expenses
{
    public class GetMonthlyExpensesQuery : IRequest<string> // Return string for PDF path
    {
    }
}
