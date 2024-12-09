using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Interfaces;

namespace OMSV1.Infrastructure.Services;

public class TokenService(IConfiguration configuration,UserManager<ApplicationUser> userManager ) : ITokenService
{
    public async Task<string> CreateToken(ApplicationUser user)
    {
       var tokenKey = configuration["TokenKey"] ?? throw new Exception("Cannot Access Token Key from app settings");
       if(tokenKey.Length <64) throw new Exception("Your tokenkey needs to be longer");
       var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
       
       if(user.UserName == null) throw new Exception("No Username For User");

       var claims = new List<Claim>
       {
        new(ClaimTypes.NameIdentifier,user.Id.ToString()),
        new(ClaimTypes.Name,user.UserName)

       };
       
       var roles = await userManager.GetRolesAsync(user);

       claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role,role)));


       var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

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

