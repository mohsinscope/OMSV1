using MediatR;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Application.Helpers;  // Assuming PagedList and PaginationParams are in this namespace
using System.Collections.Generic;

namespace OMSV1.Application.Queries.Governorates
{
    public class GetAllGovernoratesQuery : IRequest<PagedList<GovernorateDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllGovernoratesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
