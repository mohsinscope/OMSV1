using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.LOV;
using OMSV1.Application.Queries.LOV;
using OMSV1.Application.CQRS.Lov.DamagedDevice;
using OMSV1.Application.Helpers;
using System.Net;

namespace OMSV1.Application.Controllers
{
    public class DamagedDeviceTypeController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DamagedDeviceTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET ALL Damaged Device Types
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDamagedDeviceTypes()
        {
            try
            {
                var query = new GetAllDamagedDeviceTypesQuery();
                var result = await _mediator.Send(query);
                
                if (result == null || result.Count == 0)
                {
                    return ResponseHelper.CreateErrorResponse(HttpStatusCode.NotFound, "No damaged device types found.", null);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving damaged device types.", new[] { ex.Message });
            }
        }

        // GET Damaged Device Type by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDamagedDeviceType(int id)
        {
            try
            {
                var query = new GetDamagedDeviceTypeQuery { Id = id };
                var result = await _mediator.Send(query);

                if (result == null)
                    return NotFound($"Damaged device type with ID {id} not found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the damaged device type.", new[] { ex.Message });
            }
        }

        // UPDATE Damaged Device Type
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDamagedDeviceType(int id, [FromBody] UpdateDamagedDeviceTypeCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest("ID mismatch");

                var result = await _mediator.Send(command);

                if (result)
                    return NoContent();  // Successfully updated

                return NotFound($"Damaged device type with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while updating the damaged device type.", new[] { ex.Message });
            }
        }

        // ADD Damaged Device Type
        [HttpPost("add")]
        public async Task<IActionResult> AddDamagedDeviceType([FromBody] AddDamagedDeviceTypeCommand command)
        {
            try
            {
                // Validate the input command if necessary
                if (string.IsNullOrEmpty(command.Name) || string.IsNullOrEmpty(command.Description))
                {
                    return BadRequest("Name and Description are required.");
                }

                // Send the command to the handler via MediatR
                var result = await _mediator.Send(command);

                if (result)
                {
                    return Ok("Damaged device type added successfully.");
                }

                return BadRequest("Failed to add the damaged device type.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE Damaged Device Type
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDamagedDeviceType(int id)
        {
            try
            {
                var command = new DeleteDamagedDeviceTypeCommand { Id = id };
                var result = await _mediator.Send(command);

                if (result)
                    return NoContent();  // Successfully deleted

                return NotFound($"Damaged device type with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while deleting the damaged device type.", new[] { ex.Message });
            }
        }
    }
}
