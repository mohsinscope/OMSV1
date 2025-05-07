// --- ReplyDocumentWithAttachmentCommand.cs ---
using MediatR;
using Microsoft.AspNetCore.Http;
using OMSV1.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OMSV1.Application.Commands.Documents
{
    public class ReplyDocumentWithAttachmentCommand : IRequest<Guid>
    {
        // Parent linkage
        public Guid ParentDocumentId      { get; set; }

        // Reply metadata
        public string ReplyDocumentNumber { get; set; } = string.Empty;
        public string Title                { get; set; } = string.Empty;
        public DocumentType ReplyType      { get; set; }
        public Guid ProjectId              { get; set; }
        public DateTime ReplyDate          { get; set; }
        public bool RequiresReply          { get; set; }

        // Who is replying
        public Guid?       PrivatePartyId   { get; set; }    // ← already nullable
        public Guid?       SectionId        { get; set; }    // ← now nullable
        public Guid ProfileId { get; set; }

        // Content
        public ResponseType ResponseType { get; set; }
        public string?      Subject      { get; set; }

        // Status flags
        public bool IsUrgent    { get; set; }
        public bool IsImportant { get; set; }
        public bool IsNeeded    { get; set; }

        // Link IDs
        public List<Guid>? CCIds  { get; set; } = new();
        public List<Guid>   TagIds { get; set; } = new();

        // Optional notes
        public string? Notes       { get; set; }
        public Guid?   MinistryId  { get; set; }

        // Attachments
        public List<IFormFile> Files { get; set; } = default!;
    }
}
