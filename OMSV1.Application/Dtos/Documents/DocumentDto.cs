using OMSV1.Application.Dtos.Profiles;
using OMSV1.Application.Dtos.Projects;
using OMSV1.Application.Dtos.Documents;  // for DocumentCCDto, TagsDto
using OMSV1.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OMSV1.Application.Dtos.Documents
{
    public class DocumentDto
    {
        public Guid Id { get; set; }

        // Core properties
        public string DocumentNumber   { get; set; } = string.Empty;
        public string Title            { get; set; } = string.Empty;
        public DocumentType DocumentType { get; set; }
        public ResponseType ResponseType { get; set; }
        public string? Subject         { get; set; }
        public DateTime DocumentDate   { get; set; }

        // Reply / audit flags
        public bool IsRequiresReply    { get; set; }
        public bool IsReplied          { get; set; }
        public bool IsAudited          { get; set; }

        // Status flags
        public bool IsUrgent           { get; set; }
        public bool IsImportant        { get; set; }
        public bool IsNeeded           { get; set; }

        // Notes
        public string? Notes           { get; set; }

        // Foreign keys
        public Guid ProjectId          { get; set; }
        public Guid PartyId            { get; set; }
        public Guid? MinistryId        { get; set; }
        public Guid? ParentDocumentId  { get; set; }
        public Guid ProfileId          { get; set; }

        // Optional navigation DTOs
        public ProjectDto? Project     { get; set; }
        public DocumentPartyDto? Party { get; set; }
        public MinistryDto? Ministry   { get; set; }
        public ProfileDto? Profile     { get; set; }

        // Hierarchy
        public List<DocumentDto> ChildDocuments { get; set; }
            = new List<DocumentDto>();

        // Many‑to‑many link DTOs
        public List<DocumentCCDto> CcLinks { get; set; }
            = new List<DocumentCCDto>();

        public List<TagsDto> TagLinks { get; set; }
            = new List<TagsDto>();

        // Creation timestamp
        public DateTime DateCreated { get; set; }
    }
}
