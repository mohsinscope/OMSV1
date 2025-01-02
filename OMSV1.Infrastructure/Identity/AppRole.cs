using System;
using Microsoft.AspNetCore.Identity;

namespace OMSV1.Infrastructure.Identity;

public class AppRole : IdentityRole<Guid>
{

    public ICollection<AppUserRole> UserRoles { get; set; } = [];
    
        // Add permissions as a simple delimited string or related collection
    public ICollection<AppRolePermission> RolePermissions { get; set; } = new List<AppRolePermission>();


}