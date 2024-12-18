using Microsoft.AspNetCore.Mvc;
using MediatR;
using OMSV1.Application.Helpers;
using OMSV1.Application.Commands.Attendances;
using OMSV1.Application.Queries.Attendances;

namespace OMSV1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttendanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendances([FromQuery] PaginationParams paginationParams)
        {
            var query = new GetAllAttendancesQuery(paginationParams);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttendanceById(int id)
        {
            var query = new GetAttendanceByIdQuery(id);
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttendance([FromBody] CreateAttendanceCommand command)
        {
            try
            {
                // Use MediatR to handle the logic
                var id = await _mediator.Send(command);

                // Return a '201 Created' response with the ID of the newly created attendance record
                return CreatedAtAction(nameof(GetAttendanceById), new { id }, id);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur and return a 500 Internal Server Error with a message
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttendance(int id, [FromBody] UpdateAttendanceCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            var command = new DeleteAttendanceCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
