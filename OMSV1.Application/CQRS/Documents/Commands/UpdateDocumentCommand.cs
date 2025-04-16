using MediatR;
using OMSV1.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OMSV1.Application.Commands.Documents
{
    public class UpdateDocumentDetailsCommand : IRequest<Guid>
    {
        /// <summary>
        /// The Id of the document being updated.
        /// </summary>
        public Guid DocumentId { get; set; }
        
        /// <summary>
        /// The new document number.
        /// </summary>
        public required string DocumentNumber { get; set; }
        
        /// <summary>
        /// The new title for the document.
        /// </summary>
        public required string Title { get; set; }
        
        /// <summary>
        /// The new document type.
        /// </summary>
        public DocumentType DocumentType { get; set; }
        
        /// <summary>
        /// The new ProjectId for the document.
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// The new document date.
        /// </summary>
        public DateTime DocumentDate { get; set; }
        
        /// <summary>
        /// Indicates whether the document requires a reply.
        /// </summary>
        public bool IsRequiresReply { get; set; }
        
        /// <summary>
        /// The new PartyId.
        /// </summary>
        public Guid PartyId { get; set; }
        
        /// <summary>
        /// The ProfileId of the user attempting the update.
        /// Must match the document’s original ProfileId.
        /// </summary>
        public required Guid ProfileId { get; set; }
        
        /// <summary>
        /// The new subject of the document.
        /// </summary>
        public string? Subject { get; set; }
        
        /// <summary>
        /// Optional ParentDocumentId if the document is a reply.
        /// </summary>
        public Guid? ParentDocumentId { get; set; } = null;
        
        /// <summary>
        /// Updated: Collection of CC IDs.
        /// </summary>
        public List<Guid>? CCIds { get; set; } = new List<Guid>();

        /// <summary>
        /// The new response type for the document.
        /// </summary>
        public ResponseType ResponseType { get; set; }

        /// <summary>
        /// Optional notes.
        /// </summary>
        public string? Notes { get; set; }
    }
}
