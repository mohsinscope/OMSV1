using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Dtos;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Application.Controllers.User;



    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if username already exists
            var existingUser = await _userManager.FindByNameAsync(userDto.UserName);
            if (existingUser != null)
                return BadRequest($"User with username '{userDto.UserName}' already exists.");

            var user = new ApplicationUser
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Created = DateTime.UtcNow,
                LastActive = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Optionally add to roles
            if (userDto.Roles != null && userDto.Roles.Any())
            {
                foreach (var role in userDto.Roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                        return BadRequest($"Role '{role}' does not exist.");

                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            return Ok(new { Message = "User created successfully." });
        }
    }


