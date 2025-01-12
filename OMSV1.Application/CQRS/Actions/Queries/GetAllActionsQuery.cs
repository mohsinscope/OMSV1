using MediatR;
using OMSV1.Application.DTOs.Expenses;

namespace OMSV1.Application.Queries.Expenses
{
    public class GetAllActionsQuery : IRequest<List<ActionDto>>
    {
    }
}
