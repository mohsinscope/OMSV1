using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Documents;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Documents;
using System;
using System.Net;
using System.Threading.Tasks;
using OMSV1.Infrastructure.Extensions;                // For Response.AddPaginationHeader if needed
using OMSV1.Application.Helpers;                      // For ResponseHelper

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
                return ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while creating the reply document.",
                    new[] { ex.Message }
                );
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
