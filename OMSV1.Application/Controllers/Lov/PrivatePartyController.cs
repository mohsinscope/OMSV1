// Application/Controllers/Documents/PrivatePartyController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Documents;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Documents;
using OMSV1.Infrastructure.Extensions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace OMSV1.Application.Controllers.Documents
{
    public class PrivatePartyController : BaseApiController
    {
        private readonly IMediator _mediator;

        public PrivatePartyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/PrivateParty
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var query = new GetAllPrivatePartiesQuery(paginationParams);
                var pagedList = await _mediator.Send(query);

                Response.AddPaginationHeader(pagedList);
                return Ok(pagedList);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving private parties.",
                    new[] { ex.Message }
                );
            }
        }

        // GET: api/PrivateParty/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var query = new GetPrivatePartyByIdQuery(id);
                var dto = await _mediator.Send(query);
                return Ok(dto);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving the private party by ID.",
                    new[] { ex.Message }
                );
            }
        }

        // POST: api/PrivateParty
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddPrivatePartyCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return Ok(new { Message = "Private party added successfully", Id = id });
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while adding the private party.",
                    new[] { ex.Message }
                );
            }
        }

        // PUT: api/PrivateParty/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePrivatePartyCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest("PrivateParty ID mismatch.");

                var success = await _mediator.Send(command);
                if (success)
                    return Ok("Private party updated successfully.");
                else
                    return BadRequest("Failed to update private party.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while updating the private party.",
                    new[] { ex.Message }
                );
            }
        }

        // DELETE: api/PrivateParty/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var command = new DeletePrivatePartyCommand(id);
                var success = await _mediator.Send(command);
                if (success)
                    return Ok("Private party deleted successfully.");
                else
                    return BadRequest("Failed to delete private party.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while deleting the private party.",
                    new[] { ex.Message }
                );
            }
        }
    }
}
