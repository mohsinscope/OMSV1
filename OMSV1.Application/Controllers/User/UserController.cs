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
using OMSV1.Infrastructure.Persistence;
namespace OMSV1.Application.Controllers.User;


public class AccountController(UserManager<ApplicationUser> userManager,ITokenService tokenService,IMapper mapper , IMediator mediator,IPhotoService photoService,AppDbContext appDbContext) : BaseApiController
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



[HttpPost("add-attachment")]
public async Task<ActionResult<AttachmentDto>> AddAttachment(IFormFile file, OMSV1.Domain.Enums.EntityType entityType1, int entityId)
{
    // Validate the incoming file
    if (file == null || file.Length == 0) 
        return BadRequest("No file was uploaded.");

    // Upload the file to Cloudinary
    var result = await photoService.AddPhotoAsync(file);
    // Check if the DamagedDevice exists
    var damagedDeviceExists = await appDbContext.DamagedDevices
    .FirstOrDefaultAsync(dd => dd.Id == 6);

    if (damagedDeviceExists==null)
    {
        return BadRequest($"No damaged device found with ID {entityId}.");
    }
    // Create the attachment entity
    var attachmentcu = new AttachmentCU(
        fileName: file.FileName, 
        filePath: result.SecureUrl.AbsoluteUri,
        entityType: OMSV1.Domain.Enums.EntityType.DamagedDevice,  
        entityId: 1
    );

    // Save to the database
    appDbContext.AttachmentCUs.Add(attachmentcu);

    if (await appDbContext.SaveChangesAsync() > 0)
    {
        // Map the entity to a DTO for the response


        return Ok("Added Successfully");
    }

    return BadRequest("Problem adding the attachment.");
}

}





 
 