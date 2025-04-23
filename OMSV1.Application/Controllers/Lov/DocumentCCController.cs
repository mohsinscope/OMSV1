using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.DocumentCC;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.DocumentCC;
using OMSV1.Infrastructure.Extensions;
using System.Net;

namespace OMSV1.Application.Controllers.Documents
{
    public class DocumentCCController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DocumentCCController(IMediator mediator)
        {
            _mediator = mediator;
        }
        

        // Add a new DocumentCC
        [HttpPost]
        // [RequirePermission("Docum")]
        public async Task<IActionResult> AddDocumentCC([FromBody] AddDocumentCCCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { Message = "Document CC added successfully", Id = result });
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while adding the document CC.", 
                    new[] { ex.Message }
                );
            }
        }
                // GET: api/documentCC/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentCCById(Guid id)
        {
            try
            {
                // Use the constructor overload
                var query = new GetDocumentCCByIdQuery(id);
                var documentCC = await _mediator.Send(query);

                return Ok(documentCC);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError, 
                    "An error occurred while retrieving the document CC by ID.", 
                    new[] { ex.Message }
                );
            }
        }


        // Get all DocumentCC with pagination parameters
        [HttpGet]
        public async Task<IActionResult> GetAllDocumentCC([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var query = new GetAllDocumentCCQuery(paginationParams);
                var documentCC = await _mediator.Send(query);

                // Add pagination details to the response headers
                Response.AddPaginationHeader(documentCC);

                return Ok(documentCC);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving document CC.",
                    new[] { ex.Message }
                );
            }
        }
                // PUT: Update an existing DocumentCC
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocumentCC(Guid id, [FromBody] UpdateDocumentCCCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest("Document CC ID mismatch.");

                var result = await _mediator.Send(command);

                if (result)
                    return Ok("Document CC updated successfully.");
                else
                    return BadRequest("Failed to update Document CC.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while updating the document CC.",
                    new[] { ex.Message }
                );
            }
        }

        // DELETE: Delete a DocumentCC
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumentCC(Guid id)
        {
            try
            {
                var command = new DeleteDocumentCCCommand(id);
                var result = await _mediator.Send(command);

                if (result)
                    return Ok("Document CC deleted successfully.");
                else
                    return BadRequest("Failed to delete Document CC.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while deleting the document CC.",
                    new[] { ex.Message }
                );
            }
        }

    }
}
