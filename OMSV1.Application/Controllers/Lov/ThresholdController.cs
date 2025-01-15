using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Queries.Thresholds;
using OMSV1.Application.Helpers;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using OMSV1.Application.Commands.Thresholds;
using OMSV1.Application.Authorization.Attributes;

namespace OMSV1.Application.Controllers
{
    public class ThresholdController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ThresholdController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET ALL Thresholds
        [HttpGet]
        public async Task<IActionResult> GetAllThresholds()
        {
            try
            {
                var query = new GetThresholdsQuery();
                var result = await _mediator.Send(query);

                if (result == null || result.Count == 0)
                {
                    return ResponseHelper.CreateErrorResponse(HttpStatusCode.NotFound, "No thresholds found.", null);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving thresholds.", new[] { ex.Message });
            }
        }

        // GET Threshold by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetThreshold(Guid id)
        {
            try
            {
                var query = new GetThresholdByIdQuery { Id = id };
                var result = await _mediator.Send(query);

                if (result == null)
                    return NotFound($"Threshold with ID {id} not found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the threshold.", new[] { ex.Message });
            }
        }

        // UPDATE Threshold
        [HttpPut("{id}")]
        [RequirePermission("LOVt")]
        public async Task<IActionResult> UpdateThreshold(Guid id, [FromBody] UpdateThresholdCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest("ID mismatch");

                var result = await _mediator.Send(command);

                if (result)
                    return NoContent();  // Successfully updated

                return NotFound($"Threshold with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while updating the threshold.", new[] { ex.Message });
            }
        }

        // ADD Threshold
        [HttpPost]
        [RequirePermission("LOVt")]
        public async Task<IActionResult> AddThreshold([FromBody] AddThresholdCommand command)
        {
            try
            {
                // Validate the input command if necessary
                if (string.IsNullOrEmpty(command.Name) || command.MinValue < 0 || command.MaxValue <= command.MinValue)
                {
                    return BadRequest("Invalid threshold values. Ensure proper name and valid value ranges.");
                }

                // Send the command to the handler via MediatR
                var result = await _mediator.Send(command);

                if (result)
                {
                    return Ok("Threshold added successfully.");
                }

                return BadRequest("Failed to add the threshold.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE Threshold
        [HttpDelete("{id}")]
        [RequirePermission("LOVt")]
        public async Task<IActionResult> DeleteThreshold(Guid id)
        {
            try
            {
                var command = new DeleteThresholdCommand { Id = id };
                var result = await _mediator.Send(command);

                if (result)
                    return NoContent();  // Successfully deleted

                return NotFound($"Threshold with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while deleting the threshold.", new[] { ex.Message });
            }
        }
    }
}
