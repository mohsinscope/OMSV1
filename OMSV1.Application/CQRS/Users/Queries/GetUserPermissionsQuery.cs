using MediatR;

namespace OMSV1.Application.CQRS.Queries.Users
{
    public class GetUserPermissionsQuery : IRequest<UserPermissionsDto>
    {
        public Guid UserId { get; set; }

        public GetUserPermissionsQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
