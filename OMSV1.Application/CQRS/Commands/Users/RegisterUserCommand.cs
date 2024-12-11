using System;
using MediatR;
using OMSV1.Application.Dtos.User;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Commands.Users;

public class RegisterUserCommand : IRequest<UserDto>
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public string FullName { get; set; } = string.Empty;
    public int OfficeId { get; set; }
    public int GovernorateId { get; set; }
    public Position Position { get; set; }
}
