using OMSV1.Domain.Enums;
using System;

namespace OMSV1.Application.Dtos.Documents
{
    public class DocumentDto
    {
        public Guid Id { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DocumentType DocumentType { get; set; }
        
        // New property: Response type.
        public ResponseType? ResponseType { get; set; }
        
        public Guid ProjectId { get; set; }
        public DateTime DocumentDate { get; set; }
        public bool IsRequiresReply { get; set; }
        public Guid PartyId { get; set; }
        
        // New: Main creator Profile reference.
        public Guid ProfileId { get; set; }
        
        public string? Subject { get; set; }
        public Guid? ParentDocumentId { get; set; }
        public Guid? CCId { get; set; }
        
        // New status flags.
        public bool IsReplied { get; set; }
        public bool IsAudited { get; set; }

        public DateTime Datecreated { get; set; }
    }
}
