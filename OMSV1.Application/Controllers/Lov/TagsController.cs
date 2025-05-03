using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Authorization.Attributes;
using OMSV1.Application.Commands.Tags;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Tags;
using OMSV1.Infrastructure.Extensions;
using System.Net;

namespace OMSV1.Application.Controllers.Tags
{
    public class TagsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        

        // Add a new Tags
        [HttpPost]
        [RequirePermission("TAGS")]
        public async Task<IActionResult> AddTags([FromBody] AddTagsCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { Message = "Tags added successfully", Id = result });
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while adding the Tags.", 
                    new[] { ex.Message }
                );
            }
        }
                // GET: api/Tags/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagsById(Guid id)
        {
            try
            {
                // Use the constructor overload
                var query = new GetTagsByIdQuery(id);
                var tag = await _mediator.Send(query);

                return Ok(tag);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while retrieving the Tags by ID.", 
                    new[] { ex.Message }
                );
            }
        }


        // Get all Tags with pagination parameters
        [HttpGet]
        public async Task<IActionResult> GetAllTags([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var query = new GetAllTagsQuery(paginationParams);
                var tags = await _mediator.Send(query);

                // Add pagination details to the response headers
                Response.AddPaginationHeader(tags);

                return Ok(tags);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving Tags.",
                    new[] { ex.Message }
                );
            }
        }
                // PUT: Update an existing Tags
        [HttpPut("{id}")]
        [RequirePermission("TAGS")]
        public async Task<IActionResult> UpdateTags(Guid id, [FromBody] UpdateTagsCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest("Tags ID mismatch.");

                var result = await _mediator.Send(command);

                if (result)
                    return Ok("Tags updated successfully.");
                else
                    return BadRequest("Failed to update Tags.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while updating the Tags.",
                    new[] { ex.Message }
                );
            }
        }

        // DELETE: Delete a Tags
        [HttpDelete("{id}")]
        [RequirePermission("TAGS")]
        public async Task<IActionResult> DeleteTags(Guid id)
        {
            try
            {
                var command = new DeleteTagsCommand(id);
                var result = await _mediator.Send(command);

                if (result)
                    return Ok("Tags deleted successfully.");
                else
                    return BadRequest("Failed to delete Tags.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while deleting the Tags .",
                    new[] { ex.Message }
                );
            }
        }

    }
}
