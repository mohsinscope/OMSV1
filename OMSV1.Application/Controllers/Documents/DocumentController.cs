using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Documents;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Documents;
using System.Net;
using OMSV1.Infrastructure.Extensions;                // For Response.AddPaginationHeader if needed

namespace OMSV1.Application.Controllers.Documents
{
    public class DocumentController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/document with form-data
        [HttpPost]
        public async Task<IActionResult> AddDocument([FromForm] AddDocumentWithAttachmentCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetDocumentById), new { id }, new { Message = "Document created successfully", Id = id });
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(new { message = knfEx.Message });
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while creating the document.",
                    new[] { ex.Message }
                );
            }
        }
                // GET: api/document?PageNumber=1&PageSize=10
        // This endpoint returns only documents that do not have a ParentDocumentId (i.e. root documents)
        [HttpGet]
        public async Task<IActionResult> GetAllDocuments([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                // Create and send the query. It is assumed that the query handler includes a filter
                // to only return documents where ParentDocumentId == null.
                var query = new GetAllDocumentsQuery(paginationParams);
                var documents = await _mediator.Send(query);

                // Optionally, add pagination details to the response headers
                Response.AddPaginationHeader(documents);
                
                return Ok(documents);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving the documents.",
                    new[] { ex.Message }
                );
            }
        }
        // GET: api/document/{id}
        // Returns a detailed document including all child documents and attachments.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(Guid id)
        {
            try
            {
                var query = new GetDocumentByIdDetailedQuery { Id = id };
                DocumentDetailedDto documentDto = await _mediator.Send(query);
                return Ok(documentDto);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving the document.",
                    new[] { ex.Message }
                );
            }
        }
        // New endpoint to get document details by DocumentNumber
        [HttpGet("bynumber/{documentNumber}")]
        public async Task<IActionResult> GetDocumentByDocumentNumber(string documentNumber)
        {
            try
            {
                var query = new GetDocumentByDocumentNumberDetailedQuery { DocumentNumber = documentNumber };
                DocumentDetailedDto documentDto = await _mediator.Send(query);
                return Ok(documentDto);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(new { message = knfEx.Message });
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving the document.",
                    new[] { ex.Message }
                );
            }
        }
        // GET: api/document/{documentId}/history
        [HttpGet("{documentId}/history")]
        public async Task<IActionResult> GetDocumentHistoryByDocumentId(Guid documentId)
        {
            try
            {
                var query = new GetDocumentHistoryByDocumentIdQuery(documentId);
                var historyDtos = await _mediator.Send(query);
                return Ok(historyDtos);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while retrieving the document history.",
                    new[] { ex.Message }
                );
            }
        }
         // POST: api/document/{id}/reply
// POST: api/document/{id}/reply
[HttpPost("{id}/reply")]
public async Task<IActionResult> ReplyDocumentWithAttachment(Guid id, [FromForm] ReplyDocumentWithAttachmentCommand command)
{
    try
    {
        if (id != command.ParentDocumentId)
        {
            return BadRequest("URL document ID mismatch with command's ParentDocumentId.");
        }

        var replyId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetDocumentById), new { id = replyId }, new { Message = "Reply document created successfully", ReplyDocumentId = replyId });
    }
    catch (KeyNotFoundException knfEx)
    {
        return NotFound(knfEx.Message);
    }
    catch (Exception ex)
    {
        // Construct a more detailed error message.
        var detailedError = ex.Message;
        if (ex.InnerException != null)
        {
            detailedError += " | Inner Exception: " + ex.InnerException.Message;
        }
        
        // Optionally, include stack trace or other debugging details if appropriate (e.g., in a development environment).
        // detailedError += " | Stack Trace: " + ex.StackTrace;

        return ResponseHelper.CreateErrorResponse(
            HttpStatusCode.InternalServerError,
            "An error occurred while creating the reply document.",
            new[] { detailedError }
        );
            }
        }       
        // PUT: api/document/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(Guid id, [FromBody] UpdateDocumentDetailsCommand command)
        {
            // Validate that the URL document id matches the request command's document id.
            if (id != command.DocumentId)
            {
                return BadRequest("The document id in the URL does not match the document id in the request body.");
            }
            
            try
            {
                var updatedDocumentId = await _mediator.Send(command);
                return Ok(new { Message = "Document updated successfully", DocumentId = updatedDocumentId });
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new { Message = uaEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, 
                                                       "An error occurred while updating the document.", 
                                                       new[] { ex.Message }));
            }
        }
        [HttpPost("{documentId}/audit")]
        public async Task<IActionResult> MarkDocumentAsAudited(Guid documentId)
        {
            var command = new MarkDocumentAsAuditedCommand { DocumentId = documentId };
            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { DocumentId = documentId, IsAudited = true });
            else
                return BadRequest("Unable to mark the document as audited.");
        }
        [HttpPost("search")]
        public async Task<IActionResult> GetDocuments([FromBody] GetDocumentsQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                Response.AddPaginationHeader(result);  // Custom extension that adds pagination headers.
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }
        
        // DELETE: api/document/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteDocumentCommand(id));
                
                if (result)
                {
                    return Ok(new { Message = "Document and its history records deleted successfully." });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = "Deletion failed." });
                }
            }
            catch (HandlerException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError,
                                                       "An error occurred while deleting the document.",
                                                       new[] { ex.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError,
                                                       "An unexpected error occurred while deleting the document.",
                                                       new[] { ex.Message }));
            }
        }

        
        // POST: api/document/{id}/changestatus
        [HttpPost("{id}/changestatus")]
        public async Task<IActionResult> ChangeDocumentStatus(Guid id, [FromBody] ChangeDocumentStatusCommand command)
        {
            try
            {
                if (id != command.DocumentId)
                    return BadRequest("URL document ID mismatch with command's DocumentId.");

                var result = await _mediator.Send(command);
                if (result)
                {
                    return Ok("Document status changed successfully (IsRequiresReply set to false).");
                }
                return BadRequest("Failed to change document status.");
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while changing the document status.",
                    new[] { ex.Message }
                );
            }
        }


    }
}
