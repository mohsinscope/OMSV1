using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Commands.Users;
using OMSV1.Application.Dtos;
using OMSV1.Application.Dtos.User;
using OMSV1.Infrastructure.Identity;
using OMSV1.Infrastructure.Interfaces;

namespace OMSV1.Application.Controllers.User;


public class AccountController(UserManager<ApplicationUser> userManager,ITokenService tokenService,IMapper mapper) : BaseApiController
{




    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register2(RegisterUserCommand command)
    {
        var userDto = await mediator.Send(command);
        return Ok(userDto);
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

    private async Task<bool> ExistUser(string Username){
        
        return await userManager.Users.AnyAsync(x=>x.NormalizedUserName == Username.ToUpper());
    }
}






