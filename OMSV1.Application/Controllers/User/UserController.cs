using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Commands.Users;
using OMSV1.Application.CQRS.Queries.Profiles;
using OMSV1.Application.Dtos;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Application.Dtos.User;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Interfaces;

namespace OMSV1.Application.Controllers.User;


public class AccountController(UserManager<ApplicationUser> userManager,ITokenService tokenService,IMapper mapper , IMediator mediator) : BaseApiController
{

    [Authorize(Policy = "RequireAdminRole")] 
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var result = await mediator.Send(command);
        return result;
    }
        


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




    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("profiles-with-users-and-roles")]
    public async Task<ActionResult> GetProfilesWithUsersAndRoles()
    {
        var profiles = await mediator.Send(new GetProfilesWithUsersAndRolesQuery());
        return Ok(profiles);
    }

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

}





 
 