using Microsoft.AspNetCore.Mvc;
using MediatR;
using OMSV1.Application.Helpers;
using OMSV1.Application.Commands.Attendances;
using OMSV1.Application.Queries.Attendances;
using OMSV1.Application.CQRS.Attendances;
using OMSV1.Infrastructure.Extensions;
using System.Net;
using OMSV1.Application.Controllers;
using OMSV1.Application.CQRS.Attendance.Queries;
using OMSV1.Application.Authorization.Attributes;

namespace OMSV1.API.Controllers
{
    public class AttendanceController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AttendanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [RequirePermission("Ar")]
        public async Task<IActionResult> GetAllAttendances([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                // Send the pagination parameters to the query handler
                var attendance = await _mediator.Send(new GetAllAttendancesQuery(paginationParams));

                // Add pagination headers to the response
                Response.AddPaginationHeader(attendance);

                // Return the paginated result
                return Ok(attendance);  // Returns PagedList<DamagedDeviceDto>
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the attendances.", new[] { ex.Message });
            }
        }

        [HttpGet("{id}")]
        [RequirePermission("Ar")]
        public async Task<IActionResult> GetAttendanceById(Guid id)
        {
            try
            {
                var query = new GetAttendanceByIdQuery(id);
                var result = await _mediator.Send(query);
                return result != null ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the attendance.", new[] { ex.Message });
            }
        }

        [HttpPost]
        [RequirePermission("Ac")]

        public async Task<IActionResult> CreateAttendance([FromBody] CreateAttendanceCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);

                // Return 201 Created response
                return CreatedAtAction(nameof(GetAttendanceById), new { id }, id);
            }
            catch (HandlerException ex)
            {
                // Return 400 BadRequest for specific business-related exceptions
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error for unexpected exceptions
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.", new[] { ex.Message });
            }
        }

        [HttpPut("{id}")]
        [RequirePermission("Au")]

        public async Task<IActionResult> UpdateAttendance(Guid id, [FromBody] UpdateAttendanceCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [RequirePermission("Ad")]
        public async Task<IActionResult> DeleteAttendance(Guid id)
        {
            var command = new DeleteAttendanceCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("search")]
        [RequirePermission("Ar")]

        public async Task<IActionResult> GetAttendances([FromBody] GetAttendanceQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                Response.AddPaginationHeader(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error with a detailed message
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while processing your request.", new[] { ex.Message });
            }
        }

        [HttpPost("statistics/office")]
        [RequirePermission("Rr")]
        public async Task<IActionResult> GetAttendanceStatistics([FromBody] GetAttendanceStatisticsInOfficeQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("search/statistics")]
        [RequirePermission("Rr")]
        public async Task<IActionResult> GetAttendanceStatistics([FromBody] SearchAttendanceStatisticsQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error with a detailed message
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while processing your request.", new[] { ex.Message });
            }
        }
    }
    
}
