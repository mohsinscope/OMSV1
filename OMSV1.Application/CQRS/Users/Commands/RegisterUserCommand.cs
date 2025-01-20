using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Domain.Enums;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Application.Commands.Users;

public class RegisterUserCommand : IRequest<IActionResult>
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string FullName { get; set; }
    public Position Position { get; set; }
    public Guid OfficeId { get; set; }
    public Guid GovernorateId { get; set; }
    public List<string> Roles { get; set; } = new();
    public ApplicationUser? CurrentUser { get; set; } // Make it nullable

}