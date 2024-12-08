using System;
using Microsoft.AspNetCore.Identity;

namespace OMSV1.Infrastructure.Identity;

public class AppRole : IdentityRole<int>
{

    public ICollection<AppUserRole> UserRoles { get; set; } = [];

}