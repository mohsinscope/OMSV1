using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Authorization.Attributes;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Application.CQRS.Commands.DamagedDevices;
using OMSV1.Application.CQRS.DamagedDevices.Queries;
using OMSV1.Application.CQRS.Queries.DamagedDevices;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.DamagedDevices;
using OMSV1.Infrastructure.Extensions;
using System.Net;

namespace OMSV1.Application.Controllers.DamagedDevices
{
    [ApiController]
    [Route("api/[controller]")]
    public class DamagedDeviceController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DamagedDeviceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get all damaged devices with pagination
        [HttpGet]
        [RequirePermission("DDr")]
        public async Task<IActionResult> GetAllDamagedDevices([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                // Send the pagination parameters to the query handler
                var damagedDevices = await _mediator.Send(new GetAllDamagedDevicesQuery(paginationParams));

                // Add pagination headers to the response
                Response.AddPaginationHeader(damagedDevices);

                // Return the paginated result
                return Ok(damagedDevices);  // Returns PagedList<DamagedDeviceDto>
            }
            catch (Exception ex)
            {
                // Handle the error and return a custom error response
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, 
                    "An error occurred while retrieving the damaged devices.", new[] { ex.Message });
            }
        }



        // Get damaged devices by governorate with optional date filters
        [HttpGet("governorate/{governorateId}")]
        [RequirePermission("DDr")]

        public async Task<IActionResult> GetByGovernorate(
            Guid governorateId, 
            [FromQuery] DateTime? startDate,  
            [FromQuery] DateTime? endDate)
        {
            try
            {
                var query = new GetDamagedDevicesByGovernorateQuery
                {
                    GovernorateId = governorateId,
                    StartDate = startDate,
                    EndDate = endDate
                };

                var result = await _mediator.Send(query);
                return Ok(result);  // Return the result with the list of DamagedDeviceDto
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, 
                    "An error occurred while retrieving damaged devices by governorate.", new[] { ex.Message });
            }
        }



        // Get damaged devices by office with optional date filters
        [HttpGet("office/{officeId}")]
        [RequirePermission("DDr")]

        public async Task<IActionResult> GetByOffice(
            Guid officeId, 
            [FromQuery] DateTime? startDate,  
            [FromQuery] DateTime? endDate)
        {
            try
            {
                var query = new GetDamagedDevicesByOfficeQuery
                {
                    OfficeId = officeId,
                    StartDate = startDate,
                    EndDate = endDate
                };

                var result = await _mediator.Send(query);
                return Ok(result);  // Return the result with the list of DamagedDeviceDto
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, 
                    "An error occurred while retrieving damaged devices by office.", new[] { ex.Message });
            }
        }



        // Get damaged device by serial number
        [HttpGet("serial/{serialNumber}")]
        [RequirePermission("DDr")]

        public async Task<IActionResult> GetBySerialNumber(string serialNumber)
        {
            try
            {
                var query = new GetDamagedDeviceBySerialNumberQuery { SerialNumber = serialNumber };
                var result = await _mediator.Send(query);
                
                if (result == null)
                {
                    return NotFound();  // Return 404 if the device is not found
                }

                return Ok(result);  // Return the result with DamagedDeviceDto
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, 
                    "An error occurred while retrieving the damaged device by serial number.", new[] { ex.Message });
            }
        }



        // Get damaged device by ID
        [HttpGet("{id}")]
        [RequirePermission("DDr")]

        public async Task<IActionResult> GetDamagedDeviceById(Guid id)
        {
            try
            {
                var query = new GetDamagedDeviceByIdQuery(id);
                var damagedDeviceDto = await _mediator.Send(query);

                if (damagedDeviceDto == null)
                {
                    return NotFound();  // Return 404 if no device is found
                }

                return Ok(damagedDeviceDto);  // Return the DamagedDeviceDto
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while fetching the damaged device.", new[] { ex.Message });
            }
        }



        // Add a new damaged device
        [HttpPost]
        [RequirePermission("DDc")]

        public async Task<IActionResult> AddDamagedDevice([FromBody] AddDamagedDeviceCommand command)
        {
            try
            {
                var userId = User.GetUserId(); 
                command.UserId = userId;

                var id = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetDamagedDeviceById), new { id }, id);  // Return 201 Created response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");  // Catch any unhandled exceptions
            }
        }



        // Update a damaged device
        [HttpPut("{id}")]
        [RequirePermission("DamagedDevice:Update")]
        public async Task<IActionResult> UpdateDamagedDevice(Guid id, [FromBody] UpdateDamagedDeviceCommand command)
        {
            if (id != command.Id) return BadRequest("Mismatched DamagedDevice ID.");

            var isUpdated = await _mediator.Send(command);
            if (!isUpdated) return NotFound();

            return NoContent();  // Return 204 No Content if update is successful
        }



        // Delete a damaged device
        [HttpDelete("{id}")]
        [RequirePermission("DamagedDevice:Delete")]

        public async Task<IActionResult> DeleteDamagedDevice(Guid id)
        {
            try
            {
                var command = new DeleteDamagedDeviceCommand(id);
                var result = await _mediator.Send(command);

                if (!result)
                {
                    return NotFound("Damaged device not found.");  // Return 404 if device is not found
                }

                return NoContent(); // Successfully deleted, return 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");  // Catch any unhandled exceptions
            }
        }



        // Search damaged devices with filters
        [HttpPost("search")]
        [RequirePermission("DamagedDevice:read")]

        public async Task<IActionResult> GetDamagedDevices([FromBody] GetDamagedDevicesQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                Response.AddPaginationHeader(result); // Add pagination headers
                return Ok(result);  // Return the search result
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });  // Return 500 if any error occurs
            }
        }



        // Get statistics for damaged devices
        [HttpPost("search/statistics")]
        [RequirePermission("DamagedDevice:read")]

        public async Task<IActionResult> GetDamagedDeviceStatistics([FromBody] SearchDamagedDevicesStatisticsQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);  // Return the statistics result
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });  // Return 500 if any error occurs
            }
        }
    }
}
