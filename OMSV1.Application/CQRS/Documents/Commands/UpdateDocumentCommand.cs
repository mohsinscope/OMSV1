// --- UpdateDocumentWithAttachmentCommand.cs ---
using MediatR;
using Microsoft.AspNetCore.Http;
using OMSV1.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OMSV1.Application.Commands.Documents
{
    public class UpdateDocumentWithAttachmentCommand : IRequest<Guid>
    {
        // Always required to identify which doc to update
        public required Guid DocumentId { get; set; }
        public required Guid ProfileId  { get; set; }

        // All other fields are now optional
        public string? DocumentNumber    { get; set; }
        public string? Title             { get; set; }
        public DocumentType? DocumentType { get; set; }
        public ResponseType? ResponseType { get; set; }
        public string? Subject           { get; set; }
        public DateTime? DocumentDate    { get; set; }

        public bool? IsRequiresReply { get; set; }
        public bool? IsUrgent        { get; set; }
        public bool? IsImportant     { get; set; }
        public bool? IsNeeded        { get; set; }

        public Guid? ProjectId        { get; set; }
        public Guid? PrivatePartyId   { get; set; }    // ← already nullable
        public Guid? SectionId        { get; set; }    // ← now nullable
        public Guid? ParentDocumentId { get; set; }

        public List<Guid>? CCIds  { get; set; }
        public List<Guid>? TagIds { get; set; }
        public List<IFormFile>? Files { get; set; }

        public string? Notes { get; set; }
    }
}
