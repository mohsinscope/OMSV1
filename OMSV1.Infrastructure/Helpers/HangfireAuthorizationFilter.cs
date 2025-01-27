using Hangfire.Dashboard;
using System.IdentityModel.Tokens.Jwt;

namespace OMSV1.Infrastructure.Helpers;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{


    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var authHeader = httpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(authHeader))
        {
            return false;
        }        {
            // Remove the "Bearer " prefix if present
            var token = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                ? authHeader.Substring("Bearer ".Length).Trim()
                : authHeader;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            // Allow access only for the Admin role
            return role == "SuperAdmin";
        }
    }
}
