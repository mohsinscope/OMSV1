using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Application.Queries.DamagedDevices;

namespace OMSV1.Application.Controllers.DamagedDevices
{
    [ApiController]
    [Route("api/[controller]")]
    public class DamagedDeviceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DamagedDeviceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDamagedDevices()
        {
            var devices = await _mediator.Send(new GetAllDamagedDevicesQuery());
            return Ok(devices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDamagedDeviceById(int id)
        {
            var device = await _mediator.Send(new GetDamagedDeviceByIdQuery(id));
            if (device == null) return NotFound();
            return Ok(device);
        }

        [HttpPost]
        public async Task<IActionResult> AddDamagedDevice([FromBody] AddDamagedDeviceCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDamagedDeviceById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDamagedDevice(int id, [FromBody] UpdateDamagedDeviceCommand command)
        {
            if (id != command.Id) return BadRequest("Mismatched DamagedDevice ID.");

            var isUpdated = await _mediator.Send(command);
            if (!isUpdated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDamagedDevice(int id)
        {
            var isDeleted = await _mediator.Send(new DeleteDamagedDeviceCommand(id));
            if (!isDeleted) return NotFound();
            return NoContent();
        }
    }
}
