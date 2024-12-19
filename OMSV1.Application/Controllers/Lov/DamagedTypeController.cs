using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.LOV;
using OMSV1.Application.CQRS.Lov.DamagedPassport;
using OMSV1.Application.Dtos.LOV;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace OMSV1.Application.Controllers.LOV
{
    [ApiController]
    [Route("api/[controller]")]
    public class DamagedTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DamagedTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Add Damaged Type
        [HttpPost("add")]
        public async Task<ActionResult> AddDamagedType([FromBody] AddDamagedTypeCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok("Damaged Type added successfully");
            }
            return BadRequest("Failed to add Damaged Type");
        }

        // Get All Damaged Types
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<DamagedTypeDto>>> GetAllDamagedTypes()
        {
            var query = new GetAllDamagedTypesQuery();
            var damagedTypes = await _mediator.Send(query);
            return Ok(damagedTypes);
        }

        // Get a Specific Damaged Type by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<DamagedTypeDto>> GetDamagedTypeById(int id)
        {
            var query = new GetDamagedTypeByIdQuery(id);  // Assuming you have a query handler for this
            var damagedType = await _mediator.Send(query);
            if (damagedType == null)
            {
                return NotFound("Damaged Type not found");
            }
            return Ok(damagedType);
        }

        // Update Damaged Type
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateDamagedType(int id, [FromBody] UpdateDamagedTypeCommand command)
        {
            command.Id = id;  // Ensure the command has the correct ID
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok("Damaged Type updated successfully");
            }
            return BadRequest("Failed to update Damaged Type");
        }

                // Delete Damaged Type
                [HttpDelete("delete/{id}")]
            public async Task<ActionResult> DeleteDamagedType(int id)
        {
            var result = await _mediator.Send(new DeleteDamagedTypeCommand(id)); // Pass the id to the constructor
            if (result)
            {
                return Ok("Damaged Type deleted successfully");
            }
            return BadRequest("Failed to delete Damaged Type");
        }
    }
}
