using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos;
using OMSV1.Application.Dtos.User;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Interfaces;

namespace OMSV1.Application.Commands.Users;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserDto>
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

   public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Check if the username already exists
            var normalizedUsername = request.UserName.ToUpper();
            if (await _userManager.Users.AnyAsync(x => x.NormalizedUserName == normalizedUsername, cancellationToken))
            {
                throw new Exception("This username is already taken.");
            }

            // Map RegisterUserCommand to RegisterDto
            var registerDto = new RegisterDto
            {
                UserName = request.UserName,
                Email = $"{request.UserName}@example.com", // Or use a specific logic for email if required
                Password = request.Password,
                Roles = request.Roles
            };

            // Map RegisterDto to ApplicationUser
            var user = _mapper.Map<ApplicationUser>(registerDto);

            // Create the user
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                throw new Exception("Failed to create user");

            // Assign roles to the user
            if (request.Roles == null || !request.Roles.Any())
                throw new Exception("You must assign at least one role to the user");

            var roleResult = await _userManager.AddToRolesAsync(user, request.Roles);
            if (!roleResult.Succeeded)
                throw new Exception("Failed to assign roles to the user");

            // Create the profile
            var profile = new OMSV1.Domain.Entities.Profiles.Profile(
                userId: user.Id,
                fullName: request.FullName,
                position: request.Position,
                officeId: request.OfficeId,
                governorateId: request.GovernorateId
            );

            // Add profile using the generic repository
            await _profileRepository.AddAsync(profile);

            // Save all changes using the UnitOfWork
            var saveResult = await _unitOfWork.SaveAsync(cancellationToken);
            if (!saveResult)
            {
                throw new Exception("Failed to save changes to the database.");
            }

            // Return UserDto
            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
            };
        }

}

