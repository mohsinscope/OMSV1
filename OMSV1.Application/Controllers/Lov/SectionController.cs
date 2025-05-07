using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Sections;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Sections;
using OMSV1.Infrastructure.Extensions;
using System.Net;

namespace OMSV1.Application.Controllers.Sections
{
    public class SectionController : BaseApiController
    {
        private readonly IMediator _mediator;

        public SectionController(IMediator mediator)
        {
            _mediator = mediator;
        }
                // Get all Section with pagination parameters
        [HttpGet]
        public async Task<IActionResult> GetAllSections([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var query = new GetAllSectionsQuery(paginationParams);
                var Section = await _mediator.Send(query);

                // Add pagination details to the response headers
                Response.AddPaginationHeader(Section);

                return Ok(Section);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving Section.",
                    new[] { ex.Message }
                );
            }
        }

        // // // GET: api/Section/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSectionById(Guid id)
        {
            try
            {
                // Use the constructor overload
                var query = new GetSectionsByIdQuery(id);
                var Section = await _mediator.Send(query);

                return Ok(Section);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while retrieving the Section by ID.", 
                    new[] { ex.Message }
                );
            }
        }

        

        // Add a new Section
        [HttpPost]
        //[RequirePermission("LOVDOC")]

        public async Task<IActionResult> AddSection([FromBody] AddSectionCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { Message = " Section added successfully", Id = result });
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while adding the Section.", 
                    new[] { ex.Message }
                );
            }
        }
        //                 // PUT: Update an existing Section
        [HttpPut("{id}")]
        //[RequirePermission("LOVDOC")]

        public async Task<IActionResult> UpdateSection(Guid id, [FromBody] UpdateSectionCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest("Section ID mismatch.");

                var result = await _mediator.Send(command);

                if (result)
                    return Ok("Section updated successfully.");
                else
                    return BadRequest("Failed to update Section.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while updating the Section.",
                    new[] { ex.Message }
                );
            }
        }

        //         // DELETE: Delete a Section
        [HttpDelete("{id}")]
        //[RequirePermission("LOVDOC")]
        public async Task<IActionResult> DeleteSection(Guid id)
        {
            try
            {
                var command = new DeleteSectionCommand(id);
                var result = await _mediator.Send(command);

                if (result)
                    return Ok("Section deleted successfully.");
                else
                    return BadRequest("Failed to delete Section.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while deleting the Section .",
                    new[] { ex.Message }
                );
            }
        }
     

    }
}
