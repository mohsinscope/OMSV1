using System;

namespace OMSV1.Application.Dtos.User;

public class UserDto
{
    public required string Username { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime AccessTokenExpires { get; set; }
    public DateTime RefreshTokenExpires { get; set; }
}

