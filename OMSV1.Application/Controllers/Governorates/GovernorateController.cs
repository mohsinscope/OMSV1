using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Application.Queries.Governorates;

namespace OMSV1.Application.Controllers.Governorates
{
    [ApiController]
    [Route("api/[controller]")]
    public class GovernorateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GovernorateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Governorate
        [HttpGet]
        public async Task<IActionResult> GetAllGovernorates()
        {
            var governorates = await _mediator.Send(new GetAllGovernoratesQuery());
            return Ok(governorates);
        }

        // GET: api/Governorate/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGovernorateById(int id, [FromQuery] bool includeOffices = false)
        {
            if (includeOffices)
            {
                // Eager loading
                var governorate = await _mediator.Send(new GetGovernorateWithOfficesQuery(id));
                if (governorate == null) return NotFound();
                return Ok(governorate);
            }
            else
            {
                // Lazy loading
                var governorate = await _mediator.Send(new GetGovernorateByIdQuery(id));
                if (governorate == null) return NotFound();
                return Ok(governorate);
            }
        }

        // POST: api/Governorate
        [HttpPost]
        public async Task<IActionResult> AddGovernorate([FromBody] AddGovernorateCommand command)
        {
            var governorateId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetGovernorateById), new { id = governorateId }, governorateId);
        }

        // PUT: api/Governorate/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGovernorate(int id, [FromBody] UpdateGovernorateCommand command)
        {
            if (id != command.Id) return BadRequest("Mismatched Governorate ID.");

            var isUpdated = await _mediator.Send(command);
            if (!isUpdated) return NotFound();
            return NoContent();
        }

        // DELETE: api/Governorate/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGovernorate(int id)
        {
            var isDeleted = await _mediator.Send(new DeleteGovernorateCommand(id));
            if (!isDeleted) return NotFound();
            return NoContent();
        }
    }
}
