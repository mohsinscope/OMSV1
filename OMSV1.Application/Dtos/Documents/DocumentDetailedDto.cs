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
        public DocumentType DocumentType { get; set; }
        public string? Subject { get; set; }
        public bool IsRequiresReply { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime DocumentDate { get; set; }
        public Guid PartyId { get; set; }
        public Guid? CCId { get; set; }

        // Nested child documents (if any)
        public List<DocumentDetailedDto> ChildDocuments { get; set; } = new List<DocumentDetailedDto>();

    }
}
