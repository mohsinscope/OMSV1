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
using OMSV1.Application.Queries.Attachments;
using OMSV1.Application.Commands.Attachment;

namespace OMSV1.Application.Controllers
{
   
    public class AttachmentController : BaseApiController
    {
        private readonly IPhotoService photoService;
        private readonly AppDbContext appDbContext;
        private readonly IMediator mediator;

        // Inject the necessary services through the constructor
        public AttachmentController(IPhotoService photoService, AppDbContext appDbContext,IMediator mediator)
        {
            this.photoService = photoService;
            this.appDbContext = appDbContext;
            this.mediator=mediator;
        }
        // Get Attachments by Entity ID and Entity Type
   [HttpGet("{entityType}/{id}")]
public async Task<IActionResult> GetAttachmentsById(int id, string entityType)
{
    // Map string to Enum (EntityType)
    if (!Enum.TryParse(entityType, true, out EntityType parsedEntityType))
    {
        return BadRequest("Invalid entity type.");
    }

    var query = new GetAttachmentsByEntityIdQuery(id, parsedEntityType);
    var attachments = await mediator.Send(query);

    if (attachments == null || attachments.Count == 0)
    {
        return NotFound("No attachments found for the provided ID.");
    }

    return Ok(attachments);
}
       [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttachment(int id)
        {
            var command = new DeleteAttachmentCommand(id);
            var result = await mediator.Send(command);

            if (!result)
            {
                return NotFound($"Attachment with ID {id} not found.");
            }

            return Ok("Attachment deleted successfully.");
        }

        // Update Attachment (File is optional)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttachment(int id, 
            [FromForm] IFormFile file, 
            [FromForm] string fileName, 
            [FromForm] EntityType entityType, 
            [FromForm] int entityId)
        {
            var command = new UpdateAttachmentCommand(id, file, fileName, entityType, entityId);
            var updatedAttachment = await mediator.Send(command);

            if (updatedAttachment == null)
            {
                return NotFound($"Attachment with ID {id} not found.");
            }

            return Ok(updatedAttachment);
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
                    case OMSV1.Domain.Enums.EntityType.Lecture:
                    var lectureExists = await appDbContext.Lectures
                        .FirstOrDefaultAsync(dd => dd.Id == entityId);

                    if (lectureExists == null)
                    {
                        return BadRequest($"No Lecture found with ID {entityId}.");
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
