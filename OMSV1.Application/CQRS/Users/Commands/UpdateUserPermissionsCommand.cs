using MediatR;

namespace OMSV1.Application.CQRS.Commands.Users
{
    public class UpdateUserPermissionsCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public List<string> Permissions { get; set; }

        public UpdateUserPermissionsCommand(Guid userId, List<string> permissions)
        {
            UserId = userId;
            Permissions = permissions;
        }
    }
}
