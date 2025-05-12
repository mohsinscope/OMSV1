using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Application.Commands.Documents;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Documents;
using System.Net;
using OMSV1.Infrastructure.Extensions;
using OMSV1.Application.Exceptions;
using OMSV1.Application.Authorization.Attributes;
using OMSV1.Application.Queries.Attachments;
using OMSV1.Domain.Enums;
using OMSV1.Infrastructure.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;                // For Response.AddPaginationHeader if needed

namespace OMSV1.Application.Controllers.Documents
{
    public class DocumentController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IMinioService _minio;


public DocumentController(IMediator mediator, IMinioService minio)
{
    _mediator = mediator;
    _minio    = minio;
}

        // POST: api/document with form-data
// POST: api/document with form-data
// POST: api/document with form-data
[HttpPost]
[RequirePermission("DOCc")]

public async Task<IActionResult> AddDocument([FromForm] AddDocumentWithAttachmentCommand command)
{
    // 1) Pre‑validate empty GUIDs
    var invalidTagIds = command.TagIds.Where(id => id == Guid.Empty).ToList();
    var invalidCcIds  = (command.CCIds ?? new List<Guid>())
                         .Where(id => id == Guid.Empty)
                         .ToList();

    if (invalidTagIds.Any() || invalidCcIds.Any())
    {
        return BadRequest(new
        {
            code          = 400,
            message       = "Some of the TagIds or CCIds you provided are empty GUIDs.",
            invalidTagIds,
            invalidCcIds
        });
    }

    try
    {
        // 2) Dispatch to handler
        var newDocId = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetDocumentById),
            new { id = newDocId },
            new
            {
                code     = 201,
                message  = "Document created successfully",
                id       = newDocId
            }
        );
    }
    catch (KeyNotFoundException knfEx)
    {
        // 404 Not Found
        return NotFound(new
        {
            code    = 404,
            message = knfEx.Message
        });
    }
    catch (DuplicateDocumentNumberException dupEx)
    {
        return Conflict(new
        {
            code    = 409,
            message = dupEx.Message
        });
    }
    catch (ArgumentException argEx) when (argEx.Message.Contains("Ids must be valid GUIDs"))
    {
        // 400 Bad Request
        return BadRequest(new
        {
            code    = 400,
            message = "One or more TagIds or CCIds could not be processed because they were invalid GUIDs.",
            details = argEx.Message
        });
    }
    catch (Exception ex)
    {
        // 500 Internal Server Error, includes console HResult
        return StatusCode(500, new
        {
            code       = 500,
            message    = "An unexpected error occurred while creating the document.",
            errors     = new[] { ex.Message },
            errorCode  = ex.HResult
        });
    }
}
[HttpGet("{documentId}/attachment-urls")]
public async Task<IActionResult> GetAttachmentUrls(
        Guid documentId,
        [FromQuery] int expirySeconds = 3600)
{
    // 1) Pull attachment rows (they carry the FilePath)
    var attachments = await _mediator.Send(
        new GetAttachmentsByEntityIdQuery(
            documentId, EntityType.Document));

    if (attachments == null || attachments.Count == 0)
        return NotFound("No attachments found for that document.");

    // 2) Build a list of { id, url }
    var tasks = attachments.Select(async a => new
    {
        a.Id,
        url = await _minio.GetPresignedUrlAsync(a.FilePath, expirySeconds)
    });

    var urls = await Task.WhenAll(tasks);

    return Ok(urls);
}

     // GET: api/document?PageNumber=1&PageSize=10
        // This endpoint returns only documents that do not have a ParentDocumentId (i.e. root documents)
        [HttpGet]
        [RequirePermission("DOCr")]

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
[RequirePermission("DOCr")]
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
        // 404 if not found
        return NotFound(new 
        {
            error   = knfEx.Message,
            details = knfEx.StackTrace
        });
    }
    catch (Exception ex)
    {
        // Walk the InnerException chain for more context
        var errors = new List<string>();
        Exception curr = ex;
        while (curr != null)
        {
            errors.Add(curr.Message);
            curr = curr.InnerException;
        }

        return StatusCode(
            StatusCodes.Status500InternalServerError,
            new
            {
                error    = "An error occurred while retrieving the document.",
                messages = errors,
                stack    = ex.StackTrace
            }
        );
    }
}

        // New endpoint to get document details by DocumentNumber
        [HttpGet("bynumber/{documentNumber}")]
        [RequirePermission("DOCr")]
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
[RequirePermission("DOCr")]
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
        // 404 with both message and stack trace
        return NotFound(new 
        {
            error   = knfEx.Message,
            stack   = knfEx.StackTrace
        });
    }
    catch (Exception ex)
    {
        // Gather all messages from the exception chain
        var messages = new List<string>();
        for (var curr = ex; curr != null; curr = curr.InnerException)
            messages.Add(curr.Message);

        return StatusCode(
            StatusCodes.Status500InternalServerError,
            new
            {
                error    = "An error occurred while retrieving the document history.",
                messages = messages,
                stack    = ex.StackTrace
            }
        );
    }
}

         // POST: api/document/{id}/reply
// POST: api/document/{id}/reply
[HttpPost("{id}/reply")]
[RequirePermission("DOCc")]

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
    catch (DuplicateDocumentNumberException dupEx)
    {
        return Conflict(new
        {
            code    = 409,
            message = dupEx.Message
        });
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
    // PUT: api/documents/{id}
    [HttpPut("{id}")]
    [RequirePermission("DOCu")]

    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateDocument(
        Guid id,
        [FromForm] UpdateDocumentWithAttachmentCommand command)
    {
        if (id != command.DocumentId)
            return BadRequest("URL id does not match command.DocumentId");

        try
        {
            var updatedId = await _mediator.Send(command);
            return Ok(new { Message = "Document updated successfully", DocumentId = updatedId });
        }
        catch (KeyNotFoundException knf)
        {
            return NotFound(knf.Message);
        }
        catch (DuplicateDocumentNumberException dup)
        {
            return Conflict(new { code = 409, message = dup.Message });
        }
        catch (UnauthorizedAccessException ua)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, new { message = ua.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(
                (int)HttpStatusCode.InternalServerError,
                ResponseHelper.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    "An error occurred while updating the document.",
                    new[] { ex.Message }
                )
            );
        }
    }
private Guid? GetProfileIdFromClaims()
{
    // 1st try our custom claim
    var pid = User.FindFirst("profileId")?.Value
           // then fall back to the JWT subject
           ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
           // then fall back to ASP.NET’s name identifier
           ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
           ;

    return Guid.TryParse(pid, out var guid) 
        ? guid 
        : (Guid?)null;
}

    // POST /api/documents/{documentId}/audit
    [HttpPost("{documentId}/audit")]
    public async Task<IActionResult> MarkDocumentAsAudited(
        Guid documentId,
        [FromBody] AuditRequest request)
    {
        var cmd = new MarkDocumentAsAuditedCommand {
            DocumentId = documentId,
            ProfileId  = request.ProfileId
        };

        var ok = await _mediator.Send(cmd);
        if (!ok) return BadRequest("Unable to mark the document as audited.");

        return Ok(new { DocumentId = documentId, IsAudited = true });
    }
public class AuditRequest
{
    public Guid ProfileId { get; set; }
}
    // POST /api/documents/{documentId}/unaudit
    [HttpPost("{documentId}/unaudit")]
    public async Task<IActionResult> MarkDocumentAsUnAudited(
        Guid documentId,
        [FromBody] AuditRequest request)
    {
        var cmd = new UnmarkDocumentAsAuditedCommand {
            DocumentId = documentId,
            ProfileId  = request.ProfileId
        };

        var ok = await _mediator.Send(cmd);
        if (!ok) return BadRequest("Unable to mark the document as unaudited.");

        return Ok(new { DocumentId = documentId, IsAudited = false });
    }
        [HttpPost("search")]
        [RequirePermission("DOCr")]
        public async Task<IActionResult> SearchDocuments([FromBody] GetDocumentsQuery query)
        {
            try
            {
                var paged = await _mediator.Send(query);
                Response.AddPaginationHeader(paged);
                return Ok(paged);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }
                // POST api/documents/count
        [HttpPost("count")]
        [RequirePermission("DOCr")]

        public async Task<IActionResult> Count([FromBody] CountDocumentsQuery query)
        {
            try
            {
                var total = await _mediator.Send(query);
                return Ok(new { TotalCount = total });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                {
                    Message = "An error occurred while counting documents.",
                    Details = ex.Message
                });
            }
        }
        [HttpPost("search-by-links")]
        [RequirePermission("DOCr")]

        public async Task<IActionResult> SearchByLinks(
            [FromBody] SearchByLinksQuery query)
        {
            var result = await _mediator.Send(query);
            Response.AddPaginationHeader(result);
            return Ok(result);
        }
        
        // DELETE: api/document/{id}
        [HttpDelete("{id}")]
        [RequirePermission("DOCd")]

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

        
        // // POST: api/document/{id}/changestatus
        // [HttpPost("{id}/changestatus")]

        // public async Task<IActionResult> ChangeDocumentStatus(Guid id, [FromBody] ChangeDocumentStatusCommand command)
        // {
        //     try
        //     {
        //         if (id != command.DocumentId)
        //             return BadRequest("URL document ID mismatch with command's DocumentId.");

        //         var result = await _mediator.Send(command);
        //         if (result)
        //         {
        //             return Ok("Document status changed successfully (IsRequiresReply set to false).");
        //         }
        //         return BadRequest("Failed to change document status.");
        //     }
        //     catch (KeyNotFoundException knfEx)
        //     {
        //         return NotFound(knfEx.Message);
        //     }
        //     catch (Exception ex)
        //     {
        //         return ResponseHelper.CreateErrorResponse(
        //             HttpStatusCode.InternalServerError,
        //             "An error occurred while changing the document status.",
        //             new[] { ex.Message }
        //         );
        //     }
        // }


    }
}
