using MediatR;
using OMSV1.Application.Dtos.Governorates;

namespace OMSV1.Application.Queries.Governorates
{
    public class GetGovernoratesForDropdownQuery : IRequest<List<GovernorateDropdownDto>>
    {
    }
}
