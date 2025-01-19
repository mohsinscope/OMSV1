using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.CQRS.Queries.Users;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Application.CQRS.Handlers.Users
{
    public class GetUserPermissionsHandler : IRequestHandler<GetUserPermissionsQuery, UserPermissionsDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public GetUserPermissionsHandler(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

public async Task<UserPermissionsDto> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
{
    // Retrieve user by ID
    var user = await _userManager.Users
        .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
        .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

    if (user == null)
    {
        throw new KeyNotFoundException($"User with ID {request.UserId} not found.");
    }

    // Retrieve roles
    var roles = await _userManager.GetRolesAsync(user);

    // Initialize permissions dictionary
    var permissions = new Dictionary<string, List<string>>();

    // Fetch role-based permissions
    foreach (var roleName in roles)
    {
        var role = await _context.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);

        if (role == null)
        {
            continue;
        }

        foreach (var permission in role.RolePermissions)
        {
            var parts = permission.Permission.Split(':');
            if (parts.Length == 2)
            {
                var resource = parts[0];
                var action = parts[1];

                if (!permissions.ContainsKey(resource))
                {
                    permissions[resource] = new List<string>();
                }

                if (!permissions[resource].Contains(action))
                {
                    permissions[resource].Add(action);
                }
            }
        }
    }

    // Fetch user-specific permissions
    var userPermissions = await _context.UserPermissions
        .Where(up => up.UserId == request.UserId)
        .Select(up => up.Permission)
        .ToListAsync(cancellationToken);

foreach (var permission in userPermissions)
{

    // Directly add permission to a "General" category if categorization isn't needed
    if (!permissions.ContainsKey("AllPermissions"))
    {
        permissions["AllPermissions"] = new List<string>();
    }

    if (!permissions["AllPermissions"].Contains(permission))
    {
        permissions["AllPermissions"].Add(permission);
    }
}
    // Return combined roles and permissions
    return new UserPermissionsDto
    {
        Roles = roles.ToList(),
        Permissions = permissions
    };
}

    }
}
