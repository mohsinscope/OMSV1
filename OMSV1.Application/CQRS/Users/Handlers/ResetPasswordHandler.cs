using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.CQRS.Users.Commands;
using OMSV1.Application.Helpers;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Application.CQRS.Users.Handlers;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, IActionResult>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ResetPasswordHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        // Validate input
        if (request == null || string.IsNullOrWhiteSpace(request.NewPassword))
        {
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid request data.");
        }

        // Find the user
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.NotFound, "User not found.");
        }

        // Reset the password
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Password reset failed.", errors);
        }

        return ResponseHelper.CreateSuccessResponse(HttpStatusCode.OK, "Password reset successfully.");
    }
}

