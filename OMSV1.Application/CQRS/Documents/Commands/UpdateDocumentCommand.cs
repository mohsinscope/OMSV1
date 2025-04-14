using MediatR;
using OMSV1.Domain.Enums;
using System;

namespace OMSV1.Application.Commands.Documents
{
    public class UpdateDocumentCommand : IRequest<Guid>
    {
        /// <summary>
        /// The Id of the document being updated.
        /// </summary>
        public Guid DocumentId { get; set; }
        
        /// <summary>
        /// The new title for the document.
        /// </summary>
        public required string Title { get; set; }
        
        /// <summary>
        /// The new subject of the document.
        /// </summary>
        public string? Subject { get; set; }
        
        /// <summary>
        /// The new document date.
        /// </summary>
        public DateTime DocumentDate { get; set; }
        
        /// <summary>
        /// The new document type.
        /// </summary>
        public DocumentType DocumentType { get; set; }
        
        /// <summary>
        /// Indicates whether the document requires a reply.
        /// </summary>
        public bool IsRequiresReply { get; set; }
        
        /// <summary>
        /// The new response type for the document.
        /// </summary>
        public ResponseType ResponseType { get; set; }
        
        /// <summary>
        /// The ProfileId of the user attempting the update.
        /// Must match the document’s original ProfileId.
        /// </summary>
        public Guid ProfileId { get; set; }
    }
}
