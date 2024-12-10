using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.DamagedPassports;
using OMSV1.Application.Queries.DamagedPassports;

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
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDamagedPassportById), new { id }, id);
        }
    }
}
