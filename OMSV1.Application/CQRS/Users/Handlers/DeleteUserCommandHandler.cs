using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Helpers;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Application.Commands.Users;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, IActionResult>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        // Validate input
        if (request.CurrentUser == null)
        {
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, "Current user information is required.");
        }

        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "User ID is required.");
        }

        // Retrieve the user to delete
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.NotFound, $"User with ID {request.UserId} not found.");
        }

        // Ensure that the current user has permission to delete the user
        var currentUserRoles = await _userManager.GetRolesAsync(request.CurrentUser);
        if (!currentUserRoles.Contains("SuperAdmin"))
        {
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.Forbidden, "Only a SuperAdmin can delete users.");
        }

        // Delete the user
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to delete user.", errors);
        }

        // Return success response
        return ResponseHelper.CreateSuccessResponse(HttpStatusCode.OK, $"User with ID {request.UserId} has been deleted.");
    }
}
