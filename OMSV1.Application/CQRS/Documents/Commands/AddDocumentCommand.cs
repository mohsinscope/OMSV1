// --- AddDocumentWithAttachmentCommand.cs ---
using MediatR;
using Microsoft.AspNetCore.Http;
using OMSV1.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OMSV1.Application.Commands.Documents
{
    public class AddDocumentWithAttachmentCommand : IRequest<Guid>
    {
        // Core document fields
        public required string DocumentNumber { get; set; }
        public required string Title          { get; set; }
        public DocumentType DocumentType      { get; set; }
        public ResponseType ResponseType      { get; set; }
        public string? Subject                { get; set; }

        // Dates & flags
        public DateTime DocumentDate { get; set; }
        public bool     IsRequiresReply { get; set; }
        public bool     IsUrgent         { get; set; }
        public bool     IsImportant      { get; set; }
        public bool     IsNeeded         { get; set; }

        // Relationships
        public Guid ProjectId         { get; set; }
        public Guid PartyId           { get; set; }
        public required Guid ProfileId{ get; set; }
        public Guid?    ParentDocumentId { get; set; }
        public Guid?    MinistryId       { get; set; }

        // Link IDs
        public List<Guid> TagIds { get; set; } = new();
        public List<Guid>? CCIds { get; set; } = new();

        // Attachments
        public required List<IFormFile> Files { get; set; }

        // Optional notes
        public string? Notes { get; set; }
    }
}
