using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Queries;
namespace OMSV1.Application.Controllers
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
            var damagedPassports = await _mediator.Send(new GetAllDamagedPassportsQuery());
            return Ok(damagedPassports);
        }
    }
}
