using MediatR;
using OMSV1.Application.Dtos.Offices;
using System.Collections.Generic;

namespace OMSV1.Application.Queries.Offices
{
    public class GetAllOfficesQuery : IRequest<List<OfficeDto>>
    {
        // No properties required for this query
    }
}
