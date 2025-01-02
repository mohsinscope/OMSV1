using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Commands.Users;

public class RegisterUserCommand : IRequest<IActionResult>
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public Position Position { get; set; }
    public Guid OfficeId { get; set; }
    public Guid GovernorateId { get; set; }
    public List<string> Roles { get; set; } = new();
}