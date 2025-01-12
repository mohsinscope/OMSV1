using MediatR;
using OMSV1.Application.Dtos.Expenses;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Expenses;

public class GetAllExpenseTypesQuery : IRequest<PagedList<ExpenseTypeDto>>
{
    public PaginationParams PaginationParams { get; }

    public GetAllExpenseTypesQuery(PaginationParams paginationParams)
    {
        PaginationParams = paginationParams;
    }
}