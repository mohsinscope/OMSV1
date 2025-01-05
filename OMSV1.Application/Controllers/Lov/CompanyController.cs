using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Companies;
using OMSV1.Application.Commands.LectureTypes;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Companies;
using OMSV1.Infrastructure.Extensions;
using System.Net;

namespace OMSV1.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/company
        [HttpPost("add")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> AddCompany([FromBody] AddCompanyCommand command)
        {
            try
            {
                // Send the AddCompanyCommand to MediatR
                var companyId = await _mediator.Send(command);

                // Return the ID of the newly created company
                return CreatedAtAction(nameof(AddCompany), new { id = companyId }, companyId);
            }
            catch (Exception ex)
            {
                // Return an error if something goes wrong
                return BadRequest(new { message = ex.Message });
            }
        }
        // GET api/company
        [HttpGet]
        public async Task<IActionResult> GetAllCompanies([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var companies = await _mediator.Send(new GetAllCompaniesQuery(paginationParams));
                Response.AddPaginationHeader(companies);
                return Ok(companies); // Returns PagedList<CompanyDto>
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occurred while retrieving companies.", new[] { ex.Message });
            }
        }
       // DELETE: api/company/lectureType/{lectureTypeId}
        [HttpDelete("lectureType/{lectureTypeId}")]
        public async Task<IActionResult> DeleteLectureType(Guid lectureTypeId)
        {
            var command = new DeleteLectureTypeCommand(lectureTypeId);

            try
            {
                bool result = await _mediator.Send(command);

                if (result)
                {
                    return NoContent(); // Return 204 No Content if successfully deleted
                }
                else
                {
                    return NotFound($"Lecture type with ID {lectureTypeId} not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
                // DELETE: api/company/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var command = new DeleteCompanyCommand(id);

            try
            {
                // Send the command to MediatR to delete the company
                bool result = await _mediator.Send(command);

                if (result)
                {
                    return NoContent(); // Return 204 No Content if successfully deleted
                }
                else
                {
                    return NotFound($"Company with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (optional) and return a BadRequest response
                return BadRequest($"Error: {ex.Message}");
            }
        }
        // PUT: api/company/lectureType/{lectureTypeId}
        [HttpPut("lectureType/{lectureTypeId}")]
        public async Task<IActionResult> UpdateLectureType(Guid lectureTypeId, [FromBody] UpdateLectureTypeCommand command)
        {
            // Ensure the command has the correct LectureTypeId
            command.LectureTypeId = lectureTypeId;

            try
            {
                // Send the command to MediatR to update the LectureType
                bool result = await _mediator.Send(command);

                if (result)
                {
                    return NoContent(); // Return 204 No Content if successfully updated
                }
                else
                {
                    return NotFound($"Lecture type with ID {lectureTypeId} not found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (optional) and return a BadRequest response
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // POST api/company/lecture-type
        [HttpPost("add-lecture-type")]
        [Authorize(Policy = "RequireAdminRole")]

        public async Task<IActionResult> AddLectureTypeToCompany([FromBody] AddLectureTypeToCompanyCommand command)
        {
            try
            {
                // Send the AddLectureTypeToCompanyCommand to MediatR
                var lectureTypeId = await _mediator.Send(command);

                // Return the ID of the newly created LectureType
                return CreatedAtAction(nameof(AddLectureTypeToCompany), new { id = lectureTypeId }, lectureTypeId);
            }
            catch (Exception ex)
            {
                // Return an error if something goes wrong
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
