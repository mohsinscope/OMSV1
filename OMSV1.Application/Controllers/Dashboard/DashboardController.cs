using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Dashboard.Queries;         // Namespace where GetDashboardStatisticsQuery is defined
using OMSV1.Infrastructure.Extensions;                // For Response.AddPaginationHeader if needed
using OMSV1.Application.Helpers;                      // For ResponseHelper
using OMSV1.Application.Authorization.Attributes;     // For [RequirePermission]
using System;
using System.Net;
using System.Threading.Tasks;

namespace OMSV1.Application.Controllers.Dashboard
{
    // Inherit from your BaseApiController (which might include shared API controller logic)
    public class DashboardController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/dashboard/statistics
        // Returns aggregated dashboard statistics such as total offices, governorates, staff counts,
        // total damaged passports registered today, and attendance percentage.
        [HttpGet("statistics")]
        [RequirePermission("DB")]
        public async Task<IActionResult> GetDashboardStatistics()
        {
            try
            {
                // Send the dashboard query to its handler via MediatR
                var dashboardStatistics = await _mediator.Send(new GetDashboardStatisticsQuery());

                // Return 200 OK with the dashboard statistics DTO
                return Ok(dashboardStatistics);
            }
            catch (Exception ex)
            {
                // If an error occurs, return a 500 Internal Server Error with error details.
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving dashboard statistics.",
                    new[] { ex.Message }
                );
            }
        }
                // Returns the attendance percentages for each of the last seven days (including today)
        [HttpGet("last-seven-days-attendance")]
        [RequirePermission("DB")]
        public async Task<IActionResult> GetLastSevenDaysAttendance()
        {
            try
            {
                // Send the query for the last seven days attendance data via MediatR.
                var lastSevenDaysAttendance = await _mediator.Send(new GetLastSevenDaysAttendanceQuery());

                // Return 200 OK with the attendance data DTO
                return Ok(lastSevenDaysAttendance);
            }
            catch (Exception ex)
            {
                // In case of an error, return a 500 Internal Server Error with the error details.
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving the last seven days attendance data.",
                    new[] { ex.Message }
                );
            }
        }
    }
}
