using MediatR;
using OMSV1.Application.Dtos.Offices;
using System.Collections.Generic;

namespace OMSV1.Application.Queries.Offices
{
    public class GetOfficesForDropdownQuery : IRequest<List<OfficeDropdownDto>>
    {
    }
}
