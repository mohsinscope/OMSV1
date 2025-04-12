namespace OMSV1.Application.Dtos.Documents
{
    public class DocumentDto
    {
        public Guid Id { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public OMSV1.Domain.Enums.DocumentType DocumentType { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime DocumentDate { get; set; }
        public bool IsRequiresReply { get; set; }
        public Guid PartyId { get; set; }
        public string? Subject { get; set; }
        public Guid? ParentDocumentId { get; set; }
        public Guid? CCId { get; set; }
            public DateTime Datecreated { get; set; }

    }
}
