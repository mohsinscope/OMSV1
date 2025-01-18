using MediatR;
using System.Collections.Generic;

namespace OMSV1.Application.CQRS.Queries.Permissions
{
    public class GetAllPermissionsQuery : IRequest<List<string>>
    {
    }
}
