using MediatR;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Expenses;

public class GetAllMonthlyExpensesQuery : IRequest<PagedList<MonthlyExpensesDto>>
{
    public PaginationParams PaginationParams { get; }

    public GetAllMonthlyExpensesQuery(PaginationParams paginationParams)
    {
        PaginationParams = paginationParams;
    }
}
