// using System;
// using AutoMapper;
// using MediatR;
// using Microsoft.AspNetCore.Identity;
// using OMSV1.Application.Dtos.User;
// using OMSV1.Domain.Entities.Profiles;
// using OMSV1.Domain.SeedWork;
// using OMSV1.Infrastructure.Identity;
// using OMSV1.Infrastructure.Interfaces;

// namespace OMSV1.Application.Commands.Users;

// public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserDto>
// {
//     private readonly UserManager<ApplicationUser> _userManager;
//     private readonly RoleManager<AppRole> _roleManager;
//     private readonly IMapper _mapper;
//     private readonly ITokenService _tokenService;
//     private readonly IGenericRepository<OMSV1.Domain.Entities.Profiles.Profile> _profileRepository; // Using the generic repository

//     public RegisterUserHandler(
//         UserManager<ApplicationUser> userManager,
//         RoleManager<IdentityRole> roleManager,
//         IMapper mapper,
//         ITokenService tokenService,
//         IGenericRepository<OMSV1.Domain.Entities.Profiles.Profile> profileRepository,
//          )
//     {
//         _userManager = userManager;
//         _roleManager = roleManager;
//         _mapper = mapper;
//         _tokenService = tokenService;
//         _profileRepository = profileRepository;
        
//     }

//     public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
//     {
//         // Check if the username already exists
//         if (await _userManager.FindByNameAsync(request.UserName) != null)
//             throw new Exception("Username already taken");

//         // Map to ApplicationUser and create the user
//         var user = _mapper.Map<ApplicationUser>(request);
//         var result = await _userManager.CreateAsync(user, request.Password);

//         if (!result.Succeeded)
//             throw new Exception("Failed to create user");

//         // Assign roles to the user
//         if (request.Roles == null || !request.Roles.Any())
//             throw new Exception("You must assign at least one role to the user");

//         var roleResult = await _userManager.AddToRolesAsync(user, request.Roles);
//         if (!roleResult.Succeeded)
//             throw new Exception("Failed to assign roles to the user");

//         // Create the profile
//         var profile = new OMSV1.Domain.Entities.Profiles.Profile(
//             userId: user.Id, // Link to the registered user
//             fullName: request.FullName,
//             position: request.Position,
//             officeId: request.OfficeId,
//             governorateId: request.GovernorateId
//         );

//         // Save profile using the generic repository
//         await _profileRepository.AddAsync(profile);

//         // Return UserDto
//         return new UserDto
//         {
//             Username = user.UserName,
//             Token = await _tokenService.CreateToken(user),
//         };
//     }
// }

