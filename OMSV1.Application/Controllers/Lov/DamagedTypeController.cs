using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.LOV;
using OMSV1.Application.CQRS.Lov.DamagedPassport;
using OMSV1.Application.Helpers;
using System.Net;

namespace OMSV1.Application.Controllers.LOV
{
    public class DamagedTypeController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DamagedTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Add Damaged Type
        [HttpPost("add")]
        public async Task<IActionResult> AddDamagedType([FromBody] AddDamagedTypeCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if (result)
                {
                    return Ok("Damaged Type added successfully");
                }
                return BadRequest("Failed to add Damaged Type");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while adding the damaged type.", new[] { ex.Message });
            }
        }

        // Get All Damaged Types
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDamagedTypes()
        {
            try
            {
                var query = new GetAllDamagedTypesQuery();
                var damagedTypes = await _mediator.Send(query);
                return Ok(damagedTypes);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving damaged types.", new[] { ex.Message });
            }
        }

        // Get a Specific Damaged Type by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDamagedTypeById(Guid id)
        {
            try
            {
                var query = new GetDamagedTypeByIdQuery(id);  
                var damagedType = await _mediator.Send(query);
                if (damagedType == null)
                {
                    return NotFound("Damaged Type not found");
                }
                return Ok(damagedType);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the damaged type by ID.", new[] { ex.Message });
            }
        }

        // Update Damaged Type
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDamagedType(Guid id, [FromBody] UpdateDamagedTypeCommand command)
        {
            try
            {
                command.Id = id;  // Ensure the command has the correct ID
                var result = await _mediator.Send(command);
                if (result)
                {
                    return Ok("Damaged Type updated successfully");
                }
                return BadRequest("Failed to update Damaged Type");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while updating the damaged type.", new[] { ex.Message });
            }
        }

        // Delete Damaged Type
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDamagedType(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteDamagedTypeCommand(id));
                if (result)
                {
                    return Ok("Damaged Type deleted successfully");
                }
                return BadRequest("Failed to delete Damaged Type");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while deleting the damaged type.", new[] { ex.Message });
            }
        }
    }
}
