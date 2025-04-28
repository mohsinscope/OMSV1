using OMSV1.Domain.Enums;

namespace OMSV1.Application.Dtos.Documents
{
    public class DocumentDetailedDto
    {
        // Primary identity
        public Guid Id { get; set; }

        // Core fields
        public string DocumentNumber     { get; set; } = string.Empty;
        public string Title              { get; set; } = string.Empty;
        public DocumentType DocumentType { get; set; }
        public ResponseType ResponseType { get; set; }
        public string? Subject           { get; set; }

        // Reply/audit flags
        public bool IsRequiresReply      { get; set; }
        public bool IsReplied            { get; set; }
        public bool IsAudited            { get; set; }

        // Status flags
        public bool IsUrgent             { get; set; }
        public bool IsImportant          { get; set; }
        public bool IsNeeded             { get; set; }

        // Dates & notes
        public DateTime DocumentDate     { get; set; }
        public string? Notes             { get; set; }

        // Hierarchy
        public Guid? ParentDocumentId    { get; set; }
        public List<DocumentDetailedDto> ChildDocuments { get; set; } = new();

        // Foreign keys & navigations
        public Guid ProjectId            { get; set; }

        public Guid? MinistryId          { get; set; }
        public string? MinistryName      { get; set; }

        public Guid PartyId              { get; set; }
        public string? PartyName         { get; set; }
        public PartyType PartyType       { get; set; }
        public bool PartyIsOfficial      { get; set; }

        // CC recipients (link IDs)
        public List<Guid> CcIds          { get; set; } = new();
            public List<string> CcNames { get; set; } = new();


        // Tags (link IDs)
        public List<Guid> TagIds         { get; set; } = new();
    // **NEW**: flattened CC & Tag info
    public List<string> TagNames{ get; set; } = new();
        // Creator profile
        public Guid ProfileId            { get; set; }
        public string? ProfileFullName   { get; set; }

        // Creation timestamp
        public DateTime DateCreated      { get; set; }
    }
}