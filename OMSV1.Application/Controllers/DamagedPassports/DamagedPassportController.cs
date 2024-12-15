using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.DamagedPassports;
using OMSV1.Application.Queries.DamagedPassports;
using OMSV1.Application.Commands.DamagedDevices; // Assuming the DeleteDamagedPassportCommand exists here.

namespace OMSV1.Application.Controllers.DamagedPassports
{
    [ApiController]
    [Route("api/[controller]")]
    public class DamagedPassportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DamagedPassportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDamagedPassports()
        {
            var passports = await _mediator.Send(new GetAllDamagedPassportsQuery());
            return Ok(passports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDamagedPassportById(int id)
        {
            var passport = await _mediator.Send(new GetDamagedPassportByIdQuery(id));
            if (passport == null) return NotFound();
            return Ok(passport);
        }

        [HttpPost]
        public async Task<IActionResult> AddDamagedPassport([FromBody] AddDamagedPassportCommand command)
        {
            try
            {
                // Use MediatR to handle the logic
                var id = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetDamagedPassportById), new { id }, id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT method to update the damaged passport
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDamagedPassport(int id, [FromBody] UpdateDamagedPassportCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound($"DamagedPassport with ID {id} not found.");
            }

            return NoContent(); // 204 No Content, as no data is returned after a successful update
        }

        // DELETE method to delete the damaged passport
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDamagedPassport(int id)
        {
            var result = await _mediator.Send(new DeleteDamagedPassportCommand(id));

            if (!result)
            {
                return NotFound($"DamagedPassport with ID {id} not found.");
            }

            return NoContent(); // 204 No Content, as the passport has been deleted successfully
        }
    }
}
