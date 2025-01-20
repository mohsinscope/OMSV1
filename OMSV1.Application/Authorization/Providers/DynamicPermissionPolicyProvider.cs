using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using OMSV1.Application.Authorization.Requirements;
using System;
using System.Threading.Tasks;

namespace OMSV1.Application.Authorization.Providers;

public class DynamicPermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

    public DynamicPermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // Check if the policy name starts with "RequirePermission:"
        if (policyName.StartsWith("RequirePermission:", StringComparison.OrdinalIgnoreCase))
        {
            // Extract the permission from the policy name
            var permission = policyName.Substring("RequirePermission:".Length);

            // Dynamically create a policy for the required permission
            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(permission))
                .Build();

            return policy; // Non-null policy
        }

        // Fallback to default policy provider
        return await _fallbackPolicyProvider.GetPolicyAsync(policyName);
    }

    public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        var policy = await _fallbackPolicyProvider.GetDefaultPolicyAsync();

        // Ensure non-null fallback policy
        if (policy == null)
        {
            throw new InvalidOperationException("Default authorization policy cannot be null.");
        }

        return policy;
    }

    public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        // Return as nullable, aligning with the nullable interface signature
        return await _fallbackPolicyProvider.GetFallbackPolicyAsync();
    }
}
