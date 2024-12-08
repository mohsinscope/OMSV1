using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Queries.DamagedDevices;

namespace OMSV1.Application.Controllers.DamagedDevices
{
    [ApiController]
    [Route("api/[controller]")]
    public class DamagedDeviceController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllDamagedDevices()
        {
            var damagedDevices = await _mediator.Send(new GetAllDamagedDevicesQuery());
            return Ok(damagedDevices);
        }
    }
}
