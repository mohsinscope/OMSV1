using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Directorates;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Directorates;
using OMSV1.Application.Queries.Directories;
using OMSV1.Infrastructure.Extensions;
using System.Net;

namespace OMSV1.Application.Controllers.Documents
{
    public class DirectorateController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DirectorateController(IMediator mediator)
        {
            _mediator = mediator;
        }
                // Get all Directorate with pagination parameters
        [HttpGet]
        public async Task<IActionResult> GetAllDirectorates([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var query = new GetAllDirectoratesQuery(paginationParams);
                var Directorate = await _mediator.Send(query);

                // Add pagination details to the response headers
                Response.AddPaginationHeader(Directorate);

                return Ok(Directorate);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving Directorate.",
                    new[] { ex.Message }
                );
            }
        }

        // // GET: api/Directorate/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDirectorateById(Guid id)
        {
            try
            {
                // Use the constructor overload
                var query = new GetDirectorateyByIdQuery(id);
                var Directorate = await _mediator.Send(query);

                return Ok(Directorate);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while retrieving the Directorate by ID.", 
                    new[] { ex.Message }
                );
            }
        }

                // GET: api/Directorate/ByGeneralDirectorate/{directorateId}
        [HttpGet("ByGeneralDirectorate/{GeneralDirectorateId}")]
        public async Task<IActionResult> GetByGeneralDirectorate(Guid GeneralDirectorateId)
        {
            try
            {
                var query = new GetDirectoratesByGeneralDirectorateIdQuery(GeneralDirectorateId);
                var list = await _mediator.Send(query);

                if (!list.Any())
                    return NotFound($"No GeneralDirectorates found for MinistryId {GeneralDirectorateId}.");

                return Ok(list);
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving GeneralDirectorates by MinistryId.",
                    new[] { ex.Message }
                );
            }
        }


        // Add a new Directorate
        [HttpPost]
        //[RequirePermission("LOVDOC")]

        public async Task<IActionResult> AddDirectorate([FromBody] AddDirectorateCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { Message = " Directorate added successfully", Id = result });
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while adding the Directorate.", 
                    new[] { ex.Message }
                );
            }
        }
                        // PUT: Update an existing Directorate
        [HttpPut("{id}")]
        //[RequirePermission("LOVDOC")]

        public async Task<IActionResult> UpdateDirectorate(Guid id, [FromBody] UpdateDirectorateCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest("Directorate ID mismatch.");

                var result = await _mediator.Send(command);

                if (result)
                    return Ok("Directorate updated successfully.");
                else
                    return BadRequest("Failed to update Directorate.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while updating the Directorate.",
                    new[] { ex.Message }
                );
            }
        }

                // DELETE: Delete a Directorate
        [HttpDelete("{id}")]
        //[RequirePermission("LOVDOC")]
        public async Task<IActionResult> DeleteDirectorate(Guid id)
        {
            try
            {
                var command = new DeleteDirectorateCommand(id);
                var result = await _mediator.Send(command);

                if (result)
                    return Ok("Directorate deleted successfully.");
                else
                    return BadRequest("Failed to delete Directorate.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while deleting the Directorate .",
                    new[] { ex.Message }
                );
            }
        }
     

    }
}
