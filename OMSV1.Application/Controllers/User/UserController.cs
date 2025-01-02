using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Commands.Users;
using OMSV1.Application.CQRS.Queries.Profiles;
using OMSV1.Application.Dtos.User;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Application.CQRS.Users.Commands;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace OMSV1.Application.Controllers.User;


public class AccountController : BaseApiController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IMediator _mediator;

    public AccountController(UserManager<ApplicationUser> userManager, 
                           ITokenService tokenService, 
                           IMediator mediator)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mediator = mediator;
    }

    [Authorize(Policy = "RequireAdminRole")] 
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.NormalizedUserName == loginDto.UserName.ToUpper());

        if(user == null || user.UserName == null) 
            return Unauthorized("Invalid Username");
        
        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if(!result) return Unauthorized();

        var (accessToken, refreshToken, accessTokenExpires, refreshTokenExpires) = 
            await _tokenService.CreateToken(user);

        return new UserDto
        {
            Username = user.UserName,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpires = accessTokenExpires,
            RefreshTokenExpires = refreshTokenExpires
        }; 
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<UserDto>> RefreshToken(RefreshTokenRequest request)
    {
        var tokenResult = await _tokenService.RefreshToken(request.AccessToken, request.RefreshToken);
        
        if (tokenResult == null) return Unauthorized("Invalid token");

        var (accessToken, refreshToken, accessTokenExpires, refreshTokenExpires) = tokenResult.Value;

        // Get the username from the token claims
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(accessToken);
        var username = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "unique_name")?.Value;

        // if (username == null) return BadRequest("Invalid token structure");

        return new UserDto
        {
            Username = username,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpires = accessTokenExpires,
            RefreshTokenExpires = refreshTokenExpires
        };
    }


 
    // Get All Profiles With Roles Assigned To the User
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("profiles-with-users-and-roles")]
    public async Task<ActionResult> GetProfilesWithUsersAndRoles()
    {
        var profiles = await _mediator.Send(new GetProfilesWithUsersAndRolesQuery());
        return Ok(profiles);
    }


    // Update Profile Only
    // [Authorize(Policy = "RequireAdminRole")]
    // [HttpPut("{id}")]
    // public async Task<ActionResult<ProfileDto>> UpdateProfile(int id, [FromBody] UpdateProfileCommand command)
    // {
    //     if (id != command.ProfileId)
    //         return BadRequest("Profile ID mismatch.");

    //     try
    //     {
    //         var updatedProfile = await mediator.Send(command);
    //         return Ok(updatedProfile);
    //     }
    //     catch (KeyNotFoundException ex)
    //     {
    //         return NotFound(ex.Message);
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, $"Internal server error: {ex.Message}");
    //     }
    // }


    // Update Profile And User Together 
    [HttpPut("{id:int}")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserCommand command)
    {
        if (id != command.UserId)
            return BadRequest("User ID mismatch between route and body.");

        return await _mediator.Send(command);
    }


    // Change Password
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
    {
        return await _mediator.Send(command);
    }



    // Reset Password 
    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        return await _mediator.Send(command);
    }

}





 
 