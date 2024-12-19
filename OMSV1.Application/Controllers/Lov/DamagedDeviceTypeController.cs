using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.LOV;
using OMSV1.Application.Dtos;
using OMSV1.Application.Queries.LOV;
using System.Threading.Tasks;
using OMSV1.Application.CQRS.Lov.DamagedDevice;
namespace OMSV1.Application.Controllers
{
 
    public class DamagedDeviceTypeController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DamagedDeviceTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //GET ALL
          [HttpGet("all")]
        public async Task<ActionResult<List<DamagedDeviceTypeDto>>> GetAllDamagedDeviceTypes()
        {
            var query = new GetAllDamagedDeviceTypesQuery();
            var result = await _mediator.Send(query);
            
            if (result == null || result.Count == 0)
            {
                return NotFound("No damaged device types found.");
            }

            return Ok(result);
        }
        //Get By Id
      [HttpGet("{id}")]
public async Task<ActionResult<DamagedDeviceTypeDto>> GetDamagedDeviceType(int id)
{
    var query = new GetDamagedDeviceTypeQuery { Id = id };
    var result = await _mediator.Send(query);

    if (result == null)
        return NotFound();

    return Ok(result);
}
[HttpPut("{id}")]
public async Task<ActionResult> UpdateDamagedDeviceType(int id, [FromBody] UpdateDamagedDeviceTypeCommand command)
{
    if (id != command.Id)
        return BadRequest("ID mismatch");

    var result = await _mediator.Send(command);

    if (result)
        return NoContent();

    return NotFound();
}

        [HttpPost("add")]
        public async Task<ActionResult> AddDamagedDeviceType([FromBody] AddDamagedDeviceTypeCommand command)
        {
            // Validate the input command if necessary
            if (string.IsNullOrEmpty(command.Name) || string.IsNullOrEmpty(command.Description))
            {
                return BadRequest("Name and Description are required.");
            }

            // Send the command to the handler via MediatR
            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok("Damaged device type added successfully.");
            }

            return BadRequest("Failed to add the damaged device type.");
        }
        [HttpDelete("{id}")]
public async Task<ActionResult> DeleteDamagedDeviceType(int id)
{
    var command = new DeleteDamagedDeviceTypeCommand { Id = id };
    var result = await _mediator.Send(command);

    if (result)
        return NoContent();

    return NotFound();
}

    }
}
