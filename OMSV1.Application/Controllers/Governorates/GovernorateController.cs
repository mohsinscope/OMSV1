using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Queries.Governorates;
namespace OMSV1.Application.Controllers.Governorates
{
    [ApiController]
    [Route("api/[controller]")]
    public class GovernorateController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllGovernorates()
        {
            var governorates = await _mediator.Send(new GetAllGovernoratesQuery());
            return Ok(governorates);
        }
    }
}
