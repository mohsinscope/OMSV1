using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OMSV1.Application.CQRS.Users.Commands;

public class ResetPasswordCommand : IRequest<IActionResult>
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public string NewPassword { get; set; }
}
