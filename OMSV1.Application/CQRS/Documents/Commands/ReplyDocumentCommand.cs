using MediatR;
using Microsoft.AspNetCore.Http;
using OMSV1.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OMSV1.Application.Commands.Documents
{
    public class ReplyDocumentWithAttachmentCommand : IRequest<Guid>
    {
        /// <summary>
        /// The parent document to which the reply will be attached.
        /// </summary>
        public Guid ParentDocumentId { get; set; }
        
        /// <summary>
        /// The unique document number for the reply.
        /// </summary>
        public string ReplyDocumentNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// The type for the reply document (for instance, IncomingReply or OutgoingReply).
        /// </summary>
        public DocumentType ReplyType { get; set; }
        
        /// <summary>
        /// The date for the reply document.
        /// </summary>
        public DateTime ReplyDate { get; set; }
        
        /// <summary>
        /// Indicates whether the reply itself requires further response.
        /// </summary>
        public bool RequiresReply { get; set; }
        
        /// <summary>
        /// The profile (main creator) of the reply document.
        /// </summary>
        public Guid ProfileId { get; set; }
        
        /// <summary>
        /// The response type for the reply document.
        /// </summary>
        public ResponseType ResponseType { get; set; }
        
        /// <summary>
        /// Optional notes for the reply.
        /// </summary>
        public string? Notes { get; set; }
        
        /// <summary>
        /// Updated: Collection of CC IDs to set on the reply.
        /// </summary>
        public List<Guid>? CCIds { get; set; } = new List<Guid>();
        
        /// <summary>
        /// One or more file attachments.
        /// </summary>
        public List<IFormFile> File { get; set; } = new List<IFormFile>();
    }
}
