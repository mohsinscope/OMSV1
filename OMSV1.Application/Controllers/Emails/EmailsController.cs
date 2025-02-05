using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Reports;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Reports;
using OMSV1.Infrastructure.Extensions;
using System.Net;

namespace OMSV1.Application.Controllers.Reports
{
    [Authorize(Policy = "RequireSuperAdminRole")]
    public class EmailReportController : BaseApiController
    {
        private readonly IMediator _mediator;

        public EmailReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmailReports([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var emailReports = await _mediator.Send(new GetAllEmailReportsQuery(paginationParams));
                Response.AddPaginationHeader(emailReports);
                return Ok(emailReports); // Returns PagedList<EmailReportDto>
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving the email reports.",
                    new[] { ex.Message }
                );
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmailReportById(Guid id)
        {
            try
            {
                var query = new GetEmailReportByIdQuery(id);
                var emailReportDto = await _mediator.Send(query);

                if (emailReportDto == null)
                {
                    return NotFound($"EmailReport with id {id} was not found.");
                }

                return Ok(emailReportDto);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving the email report.",
                    new[] { ex.Message }
                );
            }
        }

        [HttpPost("email-report")]
        public async Task<IActionResult> AddEmailReport([FromBody] AddEmailReportCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return Ok(new { Id = id }); // Return 200 OK with the created ID
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmailReport(Guid id, [FromBody] UpdateEmailReportCommand command)
        {
            if (id != command.Id)
                return BadRequest("Mismatched EmailReport ID.");

            try
            {
                var isUpdated = await _mediator.Send(command);
                if (!isUpdated)
                    return NotFound($"EmailReport with id {id} was not found.");

                return NoContent(); // Return 204 No Content upon success
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while updating the email report.",
                    new[] { ex.Message }
                );
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmailReport(Guid id)
        {
            try
            {
                var command = new DeleteEmailReportCommand(id);
                var result = await _mediator.Send(command);

                if (!result)
                {
                    return NotFound($"EmailReport with ID {id} was not found.");
                }

                return NoContent(); // Return 204 No Content upon successful deletion
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while deleting the email report.",
                    new[] { ex.Message }
                );
            }
        }

        [HttpPost("report-type")]
        public async Task<IActionResult> AddReportType([FromBody] AddReportTypeCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return Ok(new { Id = id }); // Return 200 OK with the created ID
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("ReportTypes")]
        public async Task<IActionResult> GetAllReportTypes()
        {
            try
            {
                var query = new GetAllReportTypesQuery();
                var result = await _mediator.Send(query);
                return Ok(result); // Return 200 OK with the list of ReportTypeDto
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving report types.",
                    new[] { ex.Message }
                );
            }
        }

        [HttpPut("ReportType/{id}")]
        public async Task<IActionResult> UpdateReportType(Guid id, [FromBody] UpdateReportTypeCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Mismatched ReportType ID.");
            }

            try
            {
                // Send the update command
                var isUpdated = await _mediator.Send(command);
                if (!isUpdated)
                {
                    return NotFound($"ReportType with id {id} was not found.");
                }

                return NoContent(); // Return 204 No Content on successful update
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while updating the report type.",
                    new[] { ex.Message }
                );
            }
        }
    }
}
