using Microsoft.AspNetCore.Authorization;

namespace OMSV1.Application.Authorization.Attributes;

public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(string permission) : base()
    {
        Policy = $"RequirePermission:{permission}";
    }
}
