using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Application.Commands.Users;

public class DeleteUserCommand : IRequest<IActionResult>
{
    public required string UserId { get; set; } // The ID of the user to delete
    public ApplicationUser? CurrentUser { get; set; } // The current user performing the action
}
