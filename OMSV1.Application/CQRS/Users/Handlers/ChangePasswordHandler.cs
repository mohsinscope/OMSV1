using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.CQRS.Users.Commands;
using OMSV1.Application.Helpers;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Application.CQRS.Users.Handlers;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, IActionResult>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangePasswordHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(request.UserId) ||
            string.IsNullOrWhiteSpace(request.CurrentPassword) ||
            string.IsNullOrWhiteSpace(request.NewPassword))
        {
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "All fields are required.");
        }

        // Find the user by ID
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.NotFound, "User not found.");
        }

        // Change the password
        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Password change failed.", errors);
        }

        return new ObjectResult(new { Message = "Password changed successfully." })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}