using System;

namespace OMSV1.Application.Dtos.User;

public class LoginDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }


}