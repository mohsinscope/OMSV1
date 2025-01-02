using System;

namespace OMSV1.Application.Dtos.User;

public class RefreshTokenRequest
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
