using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Queries;

namespace OMSV1.Application.Controllers
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
            var damagedDevices = await _mediator.Send(new GetAllDamagedDevicesQuery());
            return Ok(damagedDevices);
        }
    }
}
