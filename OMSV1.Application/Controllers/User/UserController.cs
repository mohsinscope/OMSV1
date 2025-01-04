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
                           IMediator mediator,AppDbContext context)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mediator = mediator;
        _context=context;
    }

    [Authorize(Policy = "RequireAdminRole")] 
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }
      // Get User Permissions
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("{userId:Guid}/permissions")]
    public async Task<IActionResult> GetUserPermissions(Guid userId)
    {
        try
        {
            var query = new GetUserPermissionsQuery(userId);
            var permissions = await _mediator.Send(query);

            return Ok(permissions);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal Server Error", details = ex.Message });
        }
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



    //Update User Permissions
    [HttpPut("{userId}/permissions")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<IActionResult> UpdateUserPermissions(Guid userId, [FromBody] List<string> permissions)
    {
        try
        {
            var command = new UpdateUserPermissionsCommand(userId, permissions);
            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok(new { message = "Permissions updated successfully." });
            }

            return BadRequest(new { message = "Failed to update permissions." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal Server Error", details = ex.Message });
        }
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
    [HttpPut("{id:int}")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserCommand command)
    {
        if (id != command.UserId)
            return BadRequest("User ID mismatch between route and body.");

        return await _mediator.Send(command);
    }


    //add permissions to a user id
    [HttpPost("{userId}/add-permissions")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<IActionResult> AddPermissionsToUser(Guid userId, [FromBody] List<string> permissions)
    {
        try
        {
            // Validate user existence
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID '{userId}' not found." });
            }

            // Fetch existing user-specific permissions
            var existingPermissions = await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.Permission)
                .ToListAsync();

            // Add only new permissions
            foreach (var permission in permissions)
            {
                if (!existingPermissions.Contains(permission))
                {
                    _context.UserPermissions.Add(new UserPermission
                    {
                        UserId = userId,
                        Permission = permission
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Permissions added successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal Server Error", details = ex.Message });
        }
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

    
    //Add Permissions To Role
    [HttpPost("{roleName}/permissions")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<IActionResult> AddPermissionsToRole(string roleName, [FromBody] List<string> permissions)
    {
        try
        {
            // Fetch the role
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                return NotFound(new { message = $"Role '{roleName}' not found." });
            }

            // Fetch existing permissions for the role
            var existingPermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Select(rp => rp.Permission)
                .ToListAsync();

            // Add only new permissions
            foreach (var permission in permissions)
            {
                if (!existingPermissions.Contains(permission))
                {
                    _context.RolePermissions.Add(new AppRolePermission
                    {
                        RoleId = role.Id,
                        Permission = permission
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Permissions added successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal Server Error", details = ex.Message });
        }
    }

}





 
 