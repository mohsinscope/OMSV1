using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Queries;
namespace OMSV1.Application.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetAllGovernorates()
        {
            var governorates = await _mediator.Send(new GetAllGovernoratesQuery());
            return Ok(governorates);
        }
    }
}
