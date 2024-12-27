using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Lectures;
using OMSV1.Application.CQRS.Lectures.Queries;
using OMSV1.Application.Dtos.Lectures;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Lectures;
using OMSV1.Infrastructure.Extensions;
using System;
using System.Threading.Tasks;
using System.Net;

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
            try
            {
                // Send the pagination parameters to the query handler
                var lectures = await _mediator.Send(new GetAllLecturesQuery(paginationParams));

                // Add pagination headers to the response
                Response.AddPaginationHeader(lectures);

                // Return the paginated result
                return Ok(lectures);  // Returns PagedList<LectureDto>
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the lectures.", new[] { ex.Message });
            }
        }

        // GET method to retrieve a lecture by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLectureById(int id)
        {
            try
            {
                var query = new GetLectureByIdQuery(id);
                var lectureDto = await _mediator.Send(query);

                if (lectureDto == null)
                {
                    return NotFound($"Lecture with ID {id} not found.");
                }

                return Ok(lectureDto);  // Return the LectureDto
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving the lecture by ID.", new[] { ex.Message });
            }
        }

        // POST method to add a new lecture
        [HttpPost]
        public async Task<IActionResult> AddLecture([FromBody] AddLectureCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetLectureById), new { id }, id);  // Return 201 Created response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");  // Catch any unhandled exceptions
            }
        }

        // PUT method to update the lecture
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLecture(int id, [FromBody] UpdateLectureCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest("Mismatched Lecture ID.");

                var isUpdated = await _mediator.Send(command);
                if (!isUpdated)
                    return NotFound($"Lecture with ID {id} not found.");

                return NoContent();  // 204 No Content
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while updating the lecture.", new[] { ex.Message });
            }
        }

        // DELETE method to delete the lecture
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLecture(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteLectureCommand(id));

                if (!result)
                    return NotFound($"Lecture with ID {id} not found.");

                return NoContent();  // Successfully deleted
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");  // Catch any unhandled exceptions
            }
        }

        // Search for lectures with filters
        [HttpPost("search")]
        public async Task<IActionResult> GetLectures([FromBody] GetLectureQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                Response.AddPaginationHeader(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while processing your request.", new[] { ex.Message });
            }
        }
    }
}
