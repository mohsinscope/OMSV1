using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Attachment;
using System;
using System.Threading.Tasks;

namespace OMSV1.Application.Controllers{
    [ApiController]
[Route("api/[controller]")]
public class AttachmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public AttachmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

  [HttpPost("add-attachments")]
public async Task<IActionResult> AddAttachmentsToEntity([FromForm] AddAttachmentsToEntityCommand command)
{
    try
    {
        var result = await _mediator.Send(command);

        if (result)
            return Ok("Attachments added successfully.");

        return BadRequest("Failed to add attachments.");
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

}

}