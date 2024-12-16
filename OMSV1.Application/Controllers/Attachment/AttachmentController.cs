using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos;
using OMSV1.Domain.Enums;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace OMSV1.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttachmentController : ControllerBase
    {
        private readonly IPhotoService photoService;
        private readonly AppDbContext appDbContext;

        // Inject the necessary services through the constructor
        public AttachmentController(IPhotoService photoService, AppDbContext appDbContext)
        {
            this.photoService = photoService;
            this.appDbContext = appDbContext;
        }

        [HttpPost("add-attachment")]
        public async Task<ActionResult<AttachmentDto>> AddAttachment(
            [FromForm] IFormFile file, 
            [FromForm] int entityId, 
            [FromForm] OMSV1.Domain.Enums.EntityType entityType)
        {
            // Validate the incoming file and request
            if (file == null || file.Length == 0)
                return BadRequest("No file was uploaded.");

            // Upload the file to Cloudinary
            var result = await photoService.AddPhotoAsync(file);

            // Validate the entity based on type
            switch (entityType)
            {
                case OMSV1.Domain.Enums.EntityType.DamagedDevice:
                    var damagedDeviceExists = await appDbContext.DamagedDevices
                        .FirstOrDefaultAsync(dd => dd.Id == entityId);

                    if (damagedDeviceExists == null)
                    {
                        return BadRequest($"No damaged device found with ID {entityId}.");
                    }
                    break;

                case OMSV1.Domain.Enums.EntityType.DamagedPassport:
                    var damagedPassportExists = await appDbContext.DamagedPassports
                        .FirstOrDefaultAsync(dp => dp.Id == entityId);

                    if (damagedPassportExists == null)
                    {
                        return BadRequest($"No damaged passport found with ID {entityId}.");
                    }
                    break;

                // Add more cases for other entity types as needed
                default:
                    return BadRequest("Unsupported entity type.");
            }

            // Create the attachment entity
            var attachmentcu = new AttachmentCU(
                fileName: file.FileName,
                filePath: result.SecureUrl.AbsoluteUri,
                entityType: entityType,
                entityId: entityId
            );

            // Save to the database
            appDbContext.AttachmentCUs.Add(attachmentcu);

            if (await appDbContext.SaveChangesAsync() > 0)
            {
                // Return a success response
                return Ok("Added Successfully");
            }

            return BadRequest("Problem adding the attachment.");
        }
    }
}
