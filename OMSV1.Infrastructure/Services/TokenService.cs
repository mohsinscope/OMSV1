using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Infrastructure.Services;

public class TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager, AppDbContext context) : ITokenService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly AppDbContext _context = context;

    public async Task<string> CreateToken(ApplicationUser user)
    {
        var tokenKey = _configuration["TokenKey"] ?? throw new Exception("Cannot Access Token Key from app settings");
        if (tokenKey.Length < 64) throw new Exception("Your tokenkey needs to be longer");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        if (user.UserName == null) throw new Exception("No Username For User");

        // Add basic claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName)
        };

        // Add roles as claims
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // Fetch role-based permissions
        var rolePermissions = await _context.RolePermissions
            .Where(rp => roles.Contains(rp.Role.Name))
            .Select(rp => rp.Permission)
            .Distinct()
            .ToListAsync();

        // Fetch user-specific permissions
        var userPermissions = await _context.UserPermissions
            .Where(up => up.UserId == user.Id)
            .Select(up => up.Permission)
            .ToListAsync();

        // Combine role-based and user-specific permissions
        var allPermissions = rolePermissions.Union(userPermissions).Distinct();

        // Add all permissions as claims
        claims.AddRange(allPermissions.Select(permission => new Claim("Permission", permission)));

        // Create token
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
