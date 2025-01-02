using System;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Infrastructure.Interfaces;

// public interface ITokenService
// {
//     Task<string> CreateToken(ApplicationUser user);

// }

public interface ITokenService
{
    Task<(string AccessToken, string RefreshToken, DateTime AccessTokenExpires, DateTime RefreshTokenExpires)> CreateToken(ApplicationUser user);
    Task<(string AccessToken, string RefreshToken, DateTime AccessTokenExpires, DateTime RefreshTokenExpires)?> RefreshToken(string accessToken, string refreshToken);
}