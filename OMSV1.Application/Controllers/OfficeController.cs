using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Queries;

namespace OMSV1.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfficeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OfficeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOffices()
        {
            // Fully qualify the query if necessary
            var offices = await _mediator.Send(new GetAllOfficesQuery());
            return Ok(offices);
        }
    }
}
