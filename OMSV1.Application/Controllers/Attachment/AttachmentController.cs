using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Domain.Enums;
using OMSV1.Application.Queries.Attachments;
using OMSV1.Application.Commands.Attachment;
using OMSV1.Application.Helpers;
using OMSV1.Application.Commands.Attachments;

namespace OMSV1.Application.Controllers
{

    public class AttachmentController : BaseApiController
    {
        private readonly IMediator mediator;

        // Inject the necessary services through the constructor
        public AttachmentController(IMediator mediator)
        {
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
          // PUT: api/attachment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttachment(int id, 
            [FromForm] IFormFile file, 
            [FromForm] int entityId, 
            [FromForm] OMSV1.Domain.Enums.EntityType entityType)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }



            try
            {
                // Create a new UpdateAttachmentCommand based on the form data
                var request = new UpdateAttachmentCommand
                {
                    AttachmentId = id,
                    NewPhoto = file,
                    EntityId = entityId,
                    EntityType = entityType
                };

                // Send the request to update the attachment
                var result = await mediator.Send(request);

                if (!result)
                {
                    return NotFound($"Attachment with ID {id} not found.");
                }

                // Return a successful response if the update was successful
                return NoContent(); // 204 No Content
            }
            catch (HandlerException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }

         // POST: api/Attachment/add-attachment
        [HttpPost("add-attachment")]
        public async Task<IActionResult> AddAttachment([FromForm] IFormFile file, [FromForm] int entityId, [FromForm] OMSV1.Domain.Enums.EntityType entityType)
        {
            try
            {
                var command = new AddAttachmentCommand(file, entityId, entityType);
                var result = await mediator.Send(command);

                return Ok(result); // Return success message or the DTO result
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
