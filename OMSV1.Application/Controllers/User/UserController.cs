using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OMSV1.Application.Commands.Users;
using OMSV1.Application.CQRS.Queries.Profiles;
using OMSV1.Application.Dtos;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Application.Dtos.User;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Domain.Enums;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Application.Helpers;
using OMSV1.Application.CQRS.Users.Commands;
namespace OMSV1.Application.Controllers.User;


public class AccountController(UserManager<ApplicationUser> userManager,ITokenService tokenService,IMapper mapper , IMediator mediator,IPhotoService photoService,AppDbContext appDbContext) : BaseApiController
{


    // Admin Add Users
    [Authorize(Policy = "RequireAdminRole")] 
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var result = await mediator.Send(command);
        return result;
    }
        

    // Login Endpoint
    [HttpPost("Login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {


        var user = await userManager.Users
            .AsQueryable() // Ensure it's treated as IQueryable for EF Core
            .FirstOrDefaultAsync(x => x.NormalizedUserName == loginDto.UserName.ToUpper());

        if(user == null || user.UserName ==null) return Unauthorized("Invalid Username");
        
        var result = await userManager.CheckPasswordAsync(user,loginDto.Password);

        if(!result) return Unauthorized();

        return new UserDto
        {
            Username = user.UserName,
            Token = await tokenService.CreateToken(user),
        }; 

    }


 
    // Get All Profiles With Roles Assigned To the User
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("profiles-with-users-and-roles")]
    public async Task<ActionResult> GetProfilesWithUsersAndRoles()
    {
        var profiles = await mediator.Send(new GetProfilesWithUsersAndRolesQuery());
        return Ok(profiles);
    }


    // Update Profile Only
    [Authorize(Policy = "RequireAdminRole")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ProfileDto>> UpdateProfile(int id, [FromBody] UpdateProfileCommand command)
    {
        if (id != command.ProfileId)
            return BadRequest("Profile ID mismatch.");

        try
        {
            var updatedProfile = await mediator.Send(command);
            return Ok(updatedProfile);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    // Update Profile And User Together 
    [HttpPut("{id:int}")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserCommand command)
    {
        if (id != command.UserId)
            return BadRequest("User ID mismatch between route and body.");

        return await mediator.Send(command);
    }


    // Change Password
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
    {
        return await mediator.Send(command);
    }



    // Reset Password 
    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        return await mediator.Send(command);
    }

}





 
 