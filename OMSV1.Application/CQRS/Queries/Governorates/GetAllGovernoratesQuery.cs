using MediatR;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Domain.Entities.Governorates;
using System.Collections.Generic;

namespace OMSV1.Application.Queries.Governorates
{
    public class GetAllGovernoratesQuery : IRequest<List<GovernorateDto>>
    {
        // No additional properties are required for this query
    }
}