using System;
using Microsoft.AspNetCore.Identity;

namespace OMSV1.Infrastructure.Identity;

public class AppUserRole : IdentityUserRole<Guid>
{
    public ApplicationUser User { get; set; } = null!;
    public AppRole Role { get; set; } = null!;

}
