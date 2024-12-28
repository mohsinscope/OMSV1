using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OMSV1.Application.CQRS.Users.Commands;

public class ChangePasswordCommand : IRequest<IActionResult>
{
    public string UserId { get; set; } = string.Empty;
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
