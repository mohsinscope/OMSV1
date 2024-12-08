using MediatR;
using OMSV1.Domain.Entities.Offices;
using System.Collections.Generic;

namespace OMSV1.Application.Queries.Offices
{
    public class GetAllOfficesQuery : IRequest<IReadOnlyList<Office>>
    {
    }
}
