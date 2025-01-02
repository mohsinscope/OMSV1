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
using OMSV1.Infrastructure.Persistence; // Ensure AppDbContext namespace is included

namespace OMSV1.Application.Controllers.User;

public class AccountController(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService,
    IMediator mediator,
    AppDbContext context // Add AppDbContext
) : BaseApiController
{
    private readonly AppDbContext _context = context;

    // Admin Add Users
    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var result = await mediator.Send(command);
        return result;
    }

    // Login Endpoint
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await userManager.Users
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.NormalizedUserName == loginDto.UserName.ToUpper());

        if (user == null || user.UserName == null) return Unauthorized("Invalid Username");

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!result) return Unauthorized();

        return new UserDto
        {
            Username = user.UserName,
            Token = await tokenService.CreateToken(user),
        };
    }

    // Get User Permissions
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("{userId:Guid}/permissions")]
    public async Task<IActionResult> GetUserPermissions(Guid userId)
    {
        try
        {
            var query = new GetUserPermissionsQuery(userId);
            var permissions = await mediator.Send(query);

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
    //Update User Permissions
    [HttpPut("{userId}/permissions")]
    [Authorize(Policy = "RequireAdminRole")]

    public async Task<IActionResult> UpdateUserPermissions(Guid userId, [FromBody] List<string> permissions)
    {
        try
        {
            var command = new UpdateUserPermissionsCommand(userId, permissions);
            var result = await mediator.Send(command);

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
       // Get All Profiles With Roles Assigned To the User
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("profiles-with-users-and-roles")]
    public async Task<ActionResult> GetProfilesWithUsersAndRoles()
    {
        var profiles = await mediator.Send(new GetProfilesWithUsersAndRolesQuery());
        return Ok(profiles);
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

    // Add Permissions to Role
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
