using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Application.CQRS.Commands.DamagedDevices;
using OMSV1.Application.CQRS.Queries.DamagedDevices;
using OMSV1.Application.Dtos.DamagedDevices;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.DamagedDevices;
using OMSV1.Infrastructure.Extensions;

namespace OMSV1.Application.Controllers.DamagedDevices
{

    public class DamagedDeviceController : BaseApiController
    
    {
        
        private readonly IMediator _mediator;

        public DamagedDeviceController(IMediator mediator)
        {
            _mediator = mediator;
        }

         // Get All Damaged Devices with Pagination
       [HttpGet]
    public async Task<IActionResult> GetAllDamagedDevices([FromQuery] PaginationParams paginationParams)
    {
        // Send the pagination parameters to the query handler
        var damagedDevices = await _mediator.Send(new GetAllDamagedDevicesQuery(paginationParams));

        // Add pagination headers to the response
        Response.AddPaginationHeader(damagedDevices);

        // Return the paginated result
        return Ok(damagedDevices);  // Returns PagedList<DamagedDeviceDto>
    }

        
    [HttpGet("governorate/{governorateId}")]
    public async Task<ActionResult<List<DamagedDeviceDto>>> GetByGovernorate(
        int governorateId, 
        [FromQuery] DateTime? startDate,  
        [FromQuery] DateTime? endDate,
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        var query = new GetDamagedDevicesByGovernorateQuery
        {
            GovernorateId = governorateId,
            StartDate = startDate,
            EndDate = endDate,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpGet("office/{officeId}")]
    public async Task<ActionResult<List<DamagedDeviceDto>>> GetByOffice(
        int officeId, 
        [FromQuery] DateTime? startDate,  
        [FromQuery] DateTime? endDate,
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        var query = new GetDamagedDevicesByOfficeQuery
        {
            OfficeId = officeId,
            StartDate = startDate,
            EndDate = endDate,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("serial/{serialNumber}")]
    public async Task<ActionResult<DamagedDeviceDto>> GetBySerialNumber(string serialNumber)
    {
        var query = new GetDamagedDeviceBySerialNumberQuery
        {
            SerialNumber = serialNumber
        };

        var result = await _mediator.Send(query);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDamagedDeviceById(int id)
    {
        var query = new GetDamagedDeviceByIdQuery(id);
        var damagedDeviceDto = await _mediator.Send(query);

        if (damagedDeviceDto == null)
        {
            return NotFound();
        }

        return Ok(damagedDeviceDto);  // Return the DamagedDeviceDto
    }

    [HttpPost]
    public async Task<IActionResult> AddDamagedDevice([FromBody] AddDamagedDeviceCommand command)
    {
        try
        {
            var userId = User.GetUserId(); 
            command.UserId = userId;

            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDamagedDeviceById), new { id }, id);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDamagedDevice(int id, [FromBody] UpdateDamagedDeviceCommand command)
    {
        if (id != command.Id) return BadRequest("Mismatched DamagedDevice ID.");

        var isUpdated = await _mediator.Send(command);
        if (!isUpdated) return NotFound();
        return NoContent();
    }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDamagedDevice(int id)
        {
            var command = new DeleteDamagedDeviceCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound("Damaged device not found.");
            }

            return NoContent(); // Successfully deleted
        }

    }
}
