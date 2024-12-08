using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Queries.DamagedPassports;
namespace OMSV1.Application.Controllers.DamagedPassports
{
    [ApiController]
    [Route("api/[controller]")]
    public class DamagedPassportController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllDamagedPassports()
        {
            var damagedPassports = await _mediator.Send(new GetAllDamagedPassportsQuery());
            return Ok(damagedPassports);
        }
    }
}
