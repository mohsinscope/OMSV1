using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.GeneralDirectorates;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.GeneralDirectorates;
using OMSV1.Application.Queries.GeneralDirectories;
using OMSV1.Infrastructure.Extensions;
using System.Net;

namespace OMSV1.Application.Controllers.Documents
{
    public class GeneralDirectorateController : BaseApiController
    {
        private readonly IMediator _mediator;

        public GeneralDirectorateController(IMediator mediator)
        {
            _mediator = mediator;
        }
                // Get all GeneralDirectorate with pagination parameters
        [HttpGet]
        public async Task<IActionResult> GetAllGeneralDirectorates([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var query = new GetAllGeneralDirectoratesQuery(paginationParams);
                var GeneralDirectorate = await _mediator.Send(query);

                // Add pagination details to the response headers
                Response.AddPaginationHeader(GeneralDirectorate);

                return Ok(GeneralDirectorate);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving GeneralDirectorate.",
                    new[] { ex.Message }
                );
            }
        }

        // GET: api/GeneralDirectorate/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGeneralDirectorateById(Guid id)
        {
            try
            {
                // Use the constructor overload
                var query = new GetGeneralDirectorateyByIdQuery(id);
                var GeneralDirectorate = await _mediator.Send(query);

                return Ok(GeneralDirectorate);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while retrieving the GeneralDirectorate by ID.", 
                    new[] { ex.Message }
                );
            }
        }

        

        // Add a new GeneralDirectorate
        [HttpPost]
        //[RequirePermission("LOVDOC")]

        public async Task<IActionResult> AddGeneralDirectorate([FromBody] AddGeneralDirectorateCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { Message = "General Directorate added successfully", Id = result });
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while adding the GeneralDirectorate.", 
                    new[] { ex.Message }
                );
            }
        }
                        // PUT: Update an existing GeneralDirectorate
        [HttpPut("{id}")]
        //[RequirePermission("LOVDOC")]

        public async Task<IActionResult> UpdateGeneralDirectorate(Guid id, [FromBody] UpdateGeneralDirectorateCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest("GeneralDirectorate ID mismatch.");

                var result = await _mediator.Send(command);

                if (result)
                    return Ok("GeneralDirectorate updated successfully.");
                else
                    return BadRequest("Failed to update GeneralDirectorate.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while updating the GeneralDirectorate.",
                    new[] { ex.Message }
                );
            }
        }

                // DELETE: Delete a GeneralDirectorate
        [HttpDelete("{id}")]
        //[RequirePermission("LOVDOC")]
        public async Task<IActionResult> DeleteGeneralDirectorate(Guid id)
        {
            try
            {
                var command = new DeleteGeneralDirectorateCommand(id);
                var result = await _mediator.Send(command);

                if (result)
                    return Ok("GeneralDirectorate deleted successfully.");
                else
                    return BadRequest("Failed to delete GeneralDirectorate.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while deleting the GeneralDirectorate .",
                    new[] { ex.Message }
                );
            }
        }
     

    }
}
