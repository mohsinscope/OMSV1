using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using OMSV1.Application.Authorization.Requirements;


namespace OMSV1.Application.Authorization.Handlers;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        // Retrieve permissions from the user's claims
        var userPermissions = context.User.Claims
            .Where(c => c.Type == "Permission")
            .Select(c => c.Value);

        // Check if the user has the required permission
        if (userPermissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
