using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Lectures;
using OMSV1.Application.CQRS.Lectures.Queries;
using OMSV1.Application.Dtos.Lectures;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Lectures;
using OMSV1.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OMSV1.Application.Controllers.Lectures
{
    public class LectureController : BaseApiController
    {
        private readonly IMediator _mediator;

        public LectureController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get All Lectures with Pagination
        [HttpGet]
        public async Task<IActionResult> GetAllLectures([FromQuery] PaginationParams paginationParams)
        {
            // Send the pagination parameters to the query handler
            var lectures = await _mediator.Send(new GetAllLecturesQuery(paginationParams));

            // Add pagination headers to the response
            Response.AddPaginationHeader(lectures);

            // Return the paginated result
            return Ok(lectures);  // Returns PagedList<LectureDto>
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLectureById(int id)
        {
            var query = new GetLectureByIdQuery(id);
            var lectureDto = await _mediator.Send(query);

            if (lectureDto == null)
            {
                return NotFound();
            }

            return Ok(lectureDto);  // Return the LectureDto
        }

        [HttpPost]
        public async Task<IActionResult> AddLecture([FromBody] AddLectureCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetLectureById), new { id }, id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLecture(int id, [FromBody] UpdateLectureCommand command)
        {
            if (id != command.Id)
                return BadRequest("Mismatched Lecture ID.");

            var isUpdated = await _mediator.Send(command);
            if (!isUpdated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLecture(int id)
        {
            var command = new DeleteLectureCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound("Lecture not found.");
            }

            return NoContent(); // Successfully deleted
        }
        [HttpPost("search")]
        public async Task<IActionResult> GetDamagedDevices([FromBody] GetLectureQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                Response.AddPaginationHeader(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the error here (if necessary)
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }
    }
}
