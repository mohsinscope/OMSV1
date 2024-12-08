using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Queries.Offices;

namespace OMSV1.Application.Controllers.Offices
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfficeController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllOffices()
        {
            // Fully qualify the query if necessary
            var offices = await _mediator.Send(new GetAllOfficesQuery());
            return Ok(offices);
        }
    }
}
