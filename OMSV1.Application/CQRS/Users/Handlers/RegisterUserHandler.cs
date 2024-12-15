using System;
using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos;
using OMSV1.Application.Dtos.User;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Interfaces;

namespace OMSV1.Application.Commands.Users;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, IActionResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IGenericRepository<OMSV1.Domain.Entities.Profiles.Profile> _profileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserHandler(
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

    public async Task<IActionResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Null and input validation
        if (request == null)
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid request.");

        if (string.IsNullOrWhiteSpace(request.UserName))
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Username is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Password is required.");

        // Ensure roles is a list and not null
        var roles = request.Roles?.ToList() ?? new List<string>();

        // Check if the username already exists
        var normalizedUsername = request.UserName.ToUpper();
        var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == normalizedUsername, cancellationToken);
        if (existingUser != null)
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.Conflict, $"Username '{request.UserName}' is already taken.");

        // Validate roles
        if (!roles.Any())
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "At least one role must be assigned to the user.");

        // Validate role existence
        foreach (var roleName in roles)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, $"Role '{roleName}' does not exist.");
        }

        // Map RegisterUserCommand to RegisterDto
        var registerDto = new RegisterDto
        {
            UserName = request.UserName,
            Email = $"{request.UserName}@example.com", // Customize email generation as needed
            Password = request.Password,
            Roles = roles
        };

        // Map RegisterDto to ApplicationUser
        var user = _mapper.Map<ApplicationUser>(registerDto);

        // Create the user
        var userCreateResult = await _userManager.CreateAsync(user, registerDto.Password);
        if (!userCreateResult.Succeeded)
        {
            // Collect and return detailed error messages
            var errors = userCreateResult.Errors.Select(e => e.Description);
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "User creation failed", errors);
        }

        try
        {
            // Assign roles to the user
            var roleResult = await _userManager.AddToRolesAsync(user, roles);
            if (!roleResult.Succeeded)
            {
                // Delete the user if role assignment fails
                await _userManager.DeleteAsync(user);

                // Collect and return detailed error messages
                var roleErrors = roleResult.Errors.Select(e => e.Description);
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Role assignment failed", roleErrors);
            }

            // Create the profile
            var profile = new OMSV1.Domain.Entities.Profiles.Profile(
                userId: user.Id,
                fullName: request.FullName,
                position: request.Position,
                officeId: request.OfficeId,
                governorateId: request.GovernorateId
            );

            // Add profile
            await _profileRepository.AddAsync(profile);

            // Save changes
            var saveResult = await _unitOfWork.SaveAsync(cancellationToken);
            if (!saveResult)
            {
                // Delete user and rollback if save fails
                await _userManager.DeleteAsync(user);
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to save user data.");
            }

            // Return successful response with user details
            return new ObjectResult(new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
            })
            {
                StatusCode = (int)HttpStatusCode.Created
            };
        }
        catch (Exception)
        {
            // Delete the user if any unexpected error occurs
            await _userManager.DeleteAsync(user);
            return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }


}

