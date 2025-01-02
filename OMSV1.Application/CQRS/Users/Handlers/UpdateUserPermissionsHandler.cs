using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.CQRS.Commands.Users;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Application.CQRS.Handlers.Users
{
    public class UpdateUserPermissionsHandler : IRequestHandler<UpdateUserPermissionsCommand, bool>
    {
        private readonly AppDbContext _context;

        public UpdateUserPermissionsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateUserPermissionsCommand request, CancellationToken cancellationToken)
        {
            // Validate if the user exists
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {request.UserId} not found.");
            }

            // Fetch existing permissions for the user
            var existingPermissions = await _context.UserPermissions
                .Where(up => up.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            // Remove permissions not in the updated list
            var permissionsToRemove = existingPermissions
                .Where(ep => !request.Permissions.Contains(ep.Permission))
                .ToList();

            _context.UserPermissions.RemoveRange(permissionsToRemove);

            // Add new permissions that don't already exist
            var existingPermissionSet = new HashSet<string>(existingPermissions.Select(ep => ep.Permission));
            var permissionsToAdd = request.Permissions
                .Where(p => !existingPermissionSet.Contains(p))
                .Select(p => new UserPermission
                {
                    UserId = request.UserId,
                    Permission = p
                });

            _context.UserPermissions.AddRange(permissionsToAdd);

            // Save changes to the database
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
