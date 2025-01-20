using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Commands.Users;
using OMSV1.Application.CQRS.Commands.Users;
using OMSV1.Application.CQRS.Queries.Profiles;
using OMSV1.Application.CQRS.Queries.Users;
using OMSV1.Application.Dtos.User;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Application.CQRS.Users.Commands;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Application.CQRS.Profiles.Queries;
using OMSV1.Application.Dtos.Profiles;

namespace OMSV1.Application.Controllers.User;

public class AccountController : BaseApiController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IMediator _mediator;
    private readonly AppDbContext _context;

    public AccountController(UserManager<ApplicationUser> userManager, 
                           ITokenService tokenService, 
                           IMediator mediator,
                           AppDbContext context)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mediator = mediator;
        _context = context;
    }

    [Authorize(Policy = "RequireAdminRole")] 
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
    {
        // Get the current authenticated user
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        command.CurrentUser = currentUser;
        return await _mediator.Send(command);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.NormalizedUserName == loginDto.UserName.ToUpper());

        if(user == null || user.UserName == null) 
            return Unauthorized(new { message = "Invalid Username" });

        // Check if user is locked out
        if (await _userManager.IsLockedOutAsync(user))
        {
            var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
            return Unauthorized(new
            {
                message = "Account is temporarily locked due to multiple failed attempts",
                lockoutEnd = lockoutEnd,
                remainingMinutes = Math.Round((lockoutEnd.Value - DateTime.UtcNow).TotalMinutes)
            });
        }

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if(!result)
        {
            // Record failed attempt and check if should lock out
            await _userManager.AccessFailedAsync(user);
            
            var failedAttempts = await _userManager.GetAccessFailedCountAsync(user);
            var maxAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
            var remainingAttempts = maxAttempts - failedAttempts;

            if (await _userManager.IsLockedOutAsync(user))
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                return Unauthorized(new
                {
                    message = "Account has been locked due to too many failed attempts",
                    lockoutEnd = lockoutEnd,
                    remainingMinutes = Math.Round((lockoutEnd.Value - DateTime.UtcNow).TotalMinutes)
                });
            }

            return Unauthorized(new
            {
                message = "Invalid password  or username",
                remainingAttempts = remainingAttempts
            });
        }

        // Reset failed attempts count on successful login
        await _userManager.ResetAccessFailedCountAsync(user);

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

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("unlock-account/{userId}")]
    public async Task<IActionResult> UnlockAccount(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) 
            return NotFound(new { message = "User not found" });

        // Reset lockout and access failed count
        await _userManager.SetLockoutEndDateAsync(user, null);
        await _userManager.ResetAccessFailedCountAsync(user);

        return Ok(new { message = "Account unlocked successfully" });
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("locked-accounts")]
    public async Task<IActionResult> GetLockedAccounts()
    {
        var lockedUsers = await _userManager.Users
            .Where(u => u.LockoutEnd != null && u.LockoutEnd > DateTime.UtcNow)
            .Select(u => new
            {
                u.Id,
                u.UserName,
                LockoutEnd = u.LockoutEnd,
                RemainingMinutes = Math.Round((u.LockoutEnd.Value - DateTime.UtcNow).TotalMinutes)
            })
            .ToListAsync();

        return Ok(lockedUsers);
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

        if (username == null) return BadRequest("Invalid token structure");

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
    [Authorize(Policy = "RequireAdminRole")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ProfileDto>> UpdateProfile(Guid id, [FromBody] UpdateProfileCommand command)
    {
        if (id != command.ProfileId)
            return BadRequest("Profile ID mismatch.");

        try
        {
            var updatedProfile = await _mediator.Send(command);
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
    [HttpPut("{id:Guid}")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserCommand command)
    {
        if (id != command.UserId)
            return BadRequest("User ID mismatch between route and body.");

        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        command.CurrentUser = currentUser;
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
    [HttpDelete("{userId}")]
    [Authorize(Policy = "RequireSuperAdminRole")]

    public async Task<IActionResult> DeleteUser(string userId)
    {
        // Get the current logged-in user
        var currentUserName = User.Identity?.Name;
        if (string.IsNullOrEmpty(currentUserName))
        {
            return Unauthorized(new { message = "Current user is not authenticated." });
        }

        var currentUser = await _userManager.FindByNameAsync(currentUserName);
        if (currentUser == null)
        {
            return Unauthorized(new { message = "Current user not found in the system." });
        }

        // Create the command
        var command = new DeleteUserCommand
        {
            UserId = userId,
            CurrentUser = currentUser // Pass the current user to the command
        };

        return await _mediator.Send(command);
    }



}





 
 