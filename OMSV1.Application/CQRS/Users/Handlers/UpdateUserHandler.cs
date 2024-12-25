using System;
using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.CQRS.Users.Commands;
using OMSV1.Application.Dtos.User;
using OMSV1.Application.Helpers;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Profiles;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Interfaces;

namespace OMSV1.Application.CQRS.Users.Handlers;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, IActionResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IGenericRepository<OMSV1.Domain.Entities.Profiles.Profile> _profileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<AppRole> roleManager,
        IMapper mapper,
        ITokenService tokenService,
        IGenericRepository<OMSV1.Domain.Entities.Profiles.Profile> profileRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _tokenService = tokenService;
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // Null and input validation
        if (request == null)
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid request.");

        if (request.UserId <= 0)
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid User ID.");

        // Find the user
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.NotFound, "User not found.");

        try
        {
            // Update username if provided and different
            if (!string.IsNullOrWhiteSpace(request.UserName) && user.UserName != request.UserName)
            {
                // Check if the new username is already taken
                var normalizedUsername = request.UserName.ToUpper();
                var existingUser = await _userManager.Users
                    .FirstOrDefaultAsync(x => x.NormalizedUserName == normalizedUsername && 
                                            x.Id != request.UserId);
                
                if (existingUser != null)
                    return ResponseHelper.CreateErrorResponse(HttpStatusCode.Conflict, 
                        $"Username '{request.UserName}' is already taken.");

                var setUserNameResult = await _userManager.SetUserNameAsync(user, request.UserName);
                if (!setUserNameResult.Succeeded)
                {
                    var errors = setUserNameResult.Errors.Select(e => e.Description);
                    return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, 
                        "Username update failed", errors);
                }
            }

            // Update roles if provided
            if (request.Roles != null && request.Roles.Any())
            {
                // Validate roles
                foreach (var roleName in request.Roles)
                {
                    if (!await _roleManager.RoleExistsAsync(roleName))
                        return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, 
                            $"Role '{roleName}' does not exist.");
                }

                // Get current roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                
                // Remove current roles
                if (currentRoles.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        var errors = removeResult.Errors.Select(e => e.Description);
                        return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, 
                            "Failed to update roles", errors);
                    }
                }

                // Add new roles
                var addResult = await _userManager.AddToRolesAsync(user, request.Roles);
                if (!addResult.Succeeded)
                {
                    var errors = addResult.Errors.Select(e => e.Description);
                    return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, 
                        "Failed to update roles", errors);
                }
            }

            // Update profile
            var profileSpec = new ProfileByUserIdSpecification(request.UserId);
            var profile = await _profileRepository.SingleOrDefaultAsync(profileSpec);

            if (profile != null)
            {
                profile.UpdateProfile(
                    fullName: request.FullName ?? profile.FullName,
                    position: request.Position ,
                    officeId: request.OfficeId ?? profile.OfficeId,
                    governorateId: request.GovernorateId ?? profile.GovernorateId
                );

                await _profileRepository.UpdateAsync(profile);
            }

            // Save all changes
            var saveResult = await _unitOfWork.SaveAsync(cancellationToken);
            if (!saveResult)
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, 
                    "Failed to save user updates.");

            // Return successful response with updated user details
            return new ObjectResult(new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
            })
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, 
                "An unexpected error occurred.", new[] { ex.Message });
        }
    }
}