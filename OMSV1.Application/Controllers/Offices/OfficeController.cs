using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Offices;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Application.Queries.Offices;

namespace OMSV1.Application.Controllers.Offices
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfficeController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET: api/Office
        [HttpGet]
        public async Task<IActionResult> GetAllOffices()
        {
            var offices = await _mediator.Send(new GetAllOfficesQuery());
            return Ok(offices); // Returns List<OfficeDto>
        }

        // GET: api/Office/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOfficeById(int id)
        {
            var office = await _mediator.Send(new GetOfficeByIdQuery(id));
            if (office == null)
            {
                return NotFound($"Office with ID {id} not found.");
            }
            return Ok(office); // Returns OfficeDto
        }

        // POST: api/Office
        [HttpPost]
        public async Task<IActionResult> CreateOffice([FromBody] CreateOfficeDto officeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new AddOfficeCommand
            {
                Name = officeDto.Name,
                Code = officeDto.Code,
                ReceivingStaff = officeDto.ReceivingStaff,
                AccountStaff = officeDto.AccountStaff,
                PrintingStaff = officeDto.PrintingStaff,
                QualityStaff = officeDto.QualityStaff,
                DeliveryStaff = officeDto.DeliveryStaff,
                GovernorateId = officeDto.GovernorateId
            };

            var officeId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOfficeById), new { id = officeId }, officeId);
        }

        // PUT: api/Office/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOffice(int id, [FromBody] UpdateOfficeCommand command)
        {
            if (id != command.OfficeId)
            {
                return BadRequest("Mismatched office ID in the URL and body.");
            }

            var isUpdated = await _mediator.Send(command);
            if (!isUpdated)
            {
                return NotFound($"Office with ID {id} not found.");
            }

            return NoContent();
        }

        // DELETE: api/Office/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOffice(int id)
        {
            var isDeleted = await _mediator.Send(new DeleteOfficeCommand(id));
            if (!isDeleted)
            {
                return NotFound($"Office with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
