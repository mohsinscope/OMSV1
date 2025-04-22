using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Authorization.Attributes;
using OMSV1.Application.Commands.DocumentParties;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.DocumentParties;
using OMSV1.Infrastructure.Extensions;
using System.Net;

namespace OMSV1.Application.Controllers.Documents
{
    public class DocumentPartyController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DocumentPartyController(IMediator mediator)
        {
            _mediator = mediator;
        }
        

        // Add a new DocumentParty
        [HttpPost]
        // [RequirePermission("Docum")]
        public async Task<IActionResult> AddDocumentParty([FromBody] AddDocumentPartyCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { Message = "Document Party added successfully", Id = result });
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while adding the document party.", 
                    new[] { ex.Message }
                );
            }
        }
                // GET: api/documentparty/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentPartyById(Guid id)
        {
            try
            {
                // Use the constructor overload
                var query = new GetDocumentPartyByIdQuery(id);
                var documentParty = await _mediator.Send(query);

                return Ok(documentParty);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while retrieving the document party by ID.", 
                    new[] { ex.Message }
                );
            }
        }


        // Get all DocumentParties with pagination parameters
        [HttpGet]
        public async Task<IActionResult> GetAllDocumentParties([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var query = new GetAllDocumentPartiesQuery(paginationParams);
                var documentParties = await _mediator.Send(query);

                // Add pagination details to the response headers
                Response.AddPaginationHeader(documentParties);

                return Ok(documentParties);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving document parties.",
                    new[] { ex.Message }
                );
            }
        }
                // PUT: Update an existing DocumentParty
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocumentParty(Guid id, [FromBody] UpdateDocumentPartyCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest("Document Party ID mismatch.");

                var result = await _mediator.Send(command);

                if (result)
                    return Ok("Document Party updated successfully.");
                else
                    return BadRequest("Failed to update Document Party.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while updating the document party.",
                    new[] { ex.Message }
                );
            }
        }

        // DELETE: Delete a DocumentParty
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumentParty(Guid id)
        {
            try
            {
                var command = new DeleteDocumentPartyCommand(id);
                var result = await _mediator.Send(command);

                if (result)
                    return Ok("Document Party deleted successfully.");
                else
                    return BadRequest("Failed to delete Document Party.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while deleting the document party.",
                    new[] { ex.Message }
                );
            }
        }

    }
}
