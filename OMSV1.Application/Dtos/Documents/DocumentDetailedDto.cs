using OMSV1.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OMSV1.Application.Dtos.Documents
{
    public class DocumentDetailedDto
    {
        public Guid Id { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public OMSV1.Domain.Enums.DocumentType DocumentType { get; set; }
        
        // New property: Response type for replies/confirmations.
        public ResponseType? ResponseType { get; set; }
        
        public string? Subject { get; set; }
        public bool IsRequiresReply { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime DocumentDate { get; set; }
        public Guid PartyId { get; set; }
        public Guid? CCId { get; set; }
        
        // New: Main creator Profile reference.
        public Guid ProfileId { get; set; }

        // New status flags
        public bool IsReplied { get; set; }
        public bool IsAudited { get; set; }

        // Nested child documents (if any). 
        public List<DocumentDetailedDto> ChildDocuments { get; set; } = new List<DocumentDetailedDto>();

        public DateTime Datecreated { get; set; }
    }
}
