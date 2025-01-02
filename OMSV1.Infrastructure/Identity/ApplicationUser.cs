using System;
using Microsoft.AspNetCore.Identity;
using OMSV1.Domain.Entities.Users;

namespace OMSV1.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;
using OMSV1.Domain.Entities.Offices;

public class ApplicationUser :IdentityUser<Guid>
{
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
}
