using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.CQRS.Commands.Users;
using OMSV1.Application.CQRS.Queries.Permissions;
using OMSV1.Application.CQRS.Queries.Users;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Application.Controllers.Permission
{

    public class PermissionController : BaseApiController
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;

        public PermissionController(AppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        //all permissions
    [HttpGet("all-permissions")]
    [Authorize(Policy = "RequireSuperAdminRole")]
    public async Task<IActionResult> GetAllPermissions()
    {
        try
        {
            var query = new GetAllPermissionsQuery();
            var permissions = await _mediator.Send(query);

            return Ok(permissions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal Server Error", details = ex.Message });
        }
    }

        //Get user permisssions
    [Authorize(Policy = "RequireSuperAdminRole")]
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


    //Add permissions to user
    [HttpPost("{userId}/add-permissions")]
    [Authorize(Policy = "RequireSuperAdminRole")]
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
        //Update User Permissions
    [HttpPut("{userId}/permissions")]
    [Authorize(Policy = "RequireSuperAdminRole")]
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

    //Add Permissions To Role
    [HttpPost("{roleName}/permissions")]
    [Authorize(Policy = "RequireSuperAdminRole")]
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
    // Update Permissions for Role
    [HttpPut("role/{roleName}/permissions")]
    [Authorize(Policy = "RequireSuperAdminRole")]
    public async Task<IActionResult> UpdatePermissionsForRole(string roleName, [FromBody] List<string> permissions)
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
                .ToListAsync();

            // Remove all existing permissions
            _context.RolePermissions.RemoveRange(existingPermissions);

            // Add new permissions if the array is not empty
            if (permissions != null && permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    _context.RolePermissions.Add(new AppRolePermission
                    {
                        RoleId = role.Id,
                        Permission = permission
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Permissions updated successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal Server Error", details = ex.Message });
        }
    }

    }
}
