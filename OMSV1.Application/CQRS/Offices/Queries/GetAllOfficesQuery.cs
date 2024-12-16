using MediatR;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Application.Helpers;
using System.Collections.Generic;

namespace OMSV1.Application.Queries.Offices
{
    public class GetAllOfficesQuery : IRequest<PagedList<OfficeDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllOfficesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}

