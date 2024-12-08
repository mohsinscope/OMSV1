using MediatR;
using OMSV1.Domain.Entities.Governorates;
using System.Collections.Generic;

namespace OMSV1.Application.Queries;

public class GetAllGovernoratesQuery : IRequest<IReadOnlyList<Governorate>>
{
}
