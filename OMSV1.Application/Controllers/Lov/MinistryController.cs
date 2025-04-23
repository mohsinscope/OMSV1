using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Ministries;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Ministries;
using OMSV1.Infrastructure.Extensions;
using System.Net;

namespace OMSV1.Application.Controllers.Documents
{
    public class MinistryController : BaseApiController
    {
        private readonly IMediator _mediator;

        public MinistryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        

        // Add a new Ministry
        [HttpPost]
        // [RequirePermission("Docum")]
        public async Task<IActionResult> AddMinistry([FromBody] AddMinistryCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { Message = "Ministry added successfully", Id = result });
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while adding the Ministry.", 
                    new[] { ex.Message }
                );
            }
        }
                // GET: api/Ministry/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMinistryById(Guid id)
        {
            try
            {
                // Use the constructor overload
                var query = new GetMinistryByIdQuery(id);
                var ministry = await _mediator.Send(query);

                return Ok(ministry);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while retrieving the Ministry by ID.", 
                    new[] { ex.Message }
                );
            }
        }


        // Get all Ministries with pagination parameters
        [HttpGet]
        public async Task<IActionResult> GetAllMinistries([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var query = new GetAllMinistriesQuery(paginationParams);
                var ministries = await _mediator.Send(query);

                // Add pagination details to the response headers
                Response.AddPaginationHeader(ministries);

                return Ok(ministries);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving Ministries.",
                    new[] { ex.Message }
                );
            }
        }
                // PUT: Update an existing Ministry
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMinistry(Guid id, [FromBody] UpdateMinistryCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest("Ministry ID mismatch.");

                var result = await _mediator.Send(command);

                if (result)
                    return Ok("Ministry updated successfully.");
                else
                    return BadRequest("Failed to update Ministry.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while updating the Ministry.",
                    new[] { ex.Message }
                );
            }
        }

        // DELETE: Delete a Ministry
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMinistry(Guid id)
        {
            try
            {
                var command = new DeleteMinistryCommand(id);
                var result = await _mediator.Send(command);

                if (result)
                    return Ok("Ministry deleted successfully.");
                else
                    return BadRequest("Failed to delete Ministry.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while deleting the Ministry .",
                    new[] { ex.Message }
                );
            }
        }

    }
}
