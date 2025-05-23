// Application/Dtos/Documents/DocumentDto.cs
using System;
using System.Collections.Generic;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Dtos.Documents
{
    public class DocumentDto
    {
        public Guid Id { get; set; }

        // Core
        public string DocumentNumber    { get; set; } = string.Empty;
        public string Title             { get; set; } = string.Empty;
        public DocumentType DocumentType { get; set; }
        public ResponseType ResponseType { get; set; }
        public string? Subject          { get; set; }
        public DateTime DocumentDate    { get; set; }

        // Flags
        public bool IsRequiresReply     { get; set; }
        public bool IsReplied           { get; set; }
        public bool IsAudited           { get; set; }
        public bool IsUrgent            { get; set; }
        public bool IsImportant         { get; set; }
        public bool IsNeeded            { get; set; }

        // Notes
        public string? Notes            { get; set; }

        // Foreign-key IDs (if you still need them)
        public Guid ProjectId           { get; set; }
        public Guid? PrivatePartyId { get; set; }
        public Guid? SectionId         { get; set; }
        public string SectionName      { get; set; } = string.Empty;

        public Guid? DepartmentId    { get; set; }
        public string DepartmentName { get; set; } = string.Empty;

    // 2 up: Directorate
        public Guid? DirectorateId    { get; set; }
        public string DirectorateName { get; set; } = string.Empty;

    // 3 up: GeneralDirectorate
        public Guid? GeneralDirectorateId    { get; set; }
        public string GeneralDirectorateName { get; set; } = string.Empty;

    // 4 up: Ministry
        public Guid? MinistryId    { get; set; }
        public string MinistryName { get; set; } = string.Empty;
        public Guid? ParentDocumentId   { get; set; }
        public Guid ProfileId           { get; set; }

        // ONLY the *names* of those related entities
        public string ProjectName       { get; set; } = string.Empty;
        public string PrivatePartyName         { get; set; } = string.Empty;
        public string ProfileFullName       { get; set; } = string.Empty;

        // Child documents (unchanged)
        public List<DocumentDto> ChildDocuments { get; set; }
            = new List<DocumentDto>();

        // Many-to-many — now *just* their names
       // **NEW**: flattened CC & Tag info
    public List<Guid>   CcIds   { get; set; } = new();
    public List<string> CcNames { get; set; } = new();
    public List<Guid>   TagIds  { get; set; } = new();
    public List<string> TagNames{ get; set; } = new();

        // Metadata
        public DateTime DateCreated     { get; set; }
    }
}
