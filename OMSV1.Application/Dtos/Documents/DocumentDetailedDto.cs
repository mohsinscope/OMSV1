using System;
using System.Collections.Generic;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Dtos.Documents
{
    public class DocumentDetailedDto
    {
        // --- Identity ---
        public Guid Id { get; set; }

        // --- Core fields ---
        public string DocumentNumber     { get; set; } = string.Empty;
        public string Title              { get; set; } = string.Empty;
        public DocumentType DocumentType { get; set; }
        public ResponseType ResponseType { get; set; }
        public string? Subject           { get; set; }

        // --- Reply / audit flags ---
        public bool IsRequiresReply      { get; set; }
        public bool IsReplied            { get; set; }
        public bool IsAudited            { get; set; }

        // --- Status flags ---
        public bool IsUrgent             { get; set; }
        public bool IsImportant          { get; set; }
        public bool IsNeeded             { get; set; }

        // --- Dates & notes ---
        public DateTime DocumentDate     { get; set; }
        public string? Notes             { get; set; }

        // --- Hierarchy within documents ---
        public Guid? ParentDocumentId    { get; set; }
        public List<DocumentDetailedDto> ChildDocuments { get; set; } = new();

        // --- Project ---
        public Guid   ProjectId          { get; set; }
        public string ProjectName        { get; set; } = string.Empty;

        // --- Section (leaf) ---
        public Guid?  SectionId          { get; set; }
        public string SectionName        { get; set; } = string.Empty;

        // --- Department (1 up) ---
        public Guid?  DepartmentId       { get; set; }
        public string DepartmentName     { get; set; } = string.Empty;

        // --- Directorate (2 up) ---
        public Guid?  DirectorateId      { get; set; }
        public string DirectorateName    { get; set; } = string.Empty;

        // --- GeneralDirectorate (3 up) ---
        public Guid?  GeneralDirectorateId   { get; set; }
        public string GeneralDirectorateName { get; set; } = string.Empty;

        // --- Ministry (4 up) ---
        public Guid?  MinistryId         { get; set; }
        public string MinistryName       { get; set; } = string.Empty;

        // --- Private party (if any) ---
        public Guid?  PrivatePartyId     { get; set; }
        public string PrivatePartyName   { get; set; } = string.Empty;

        // --- Creator profile ---
        public Guid   ProfileId          { get; set; }
        public string ProfileFullName    { get; set; } = string.Empty;

        // --- CC recipients ---
        public List<Guid>   CcIds        { get; set; } = new();
        public List<string> CcNames      { get; set; } = new();

        // --- Tags ---
        public List<Guid>   TagIds       { get; set; } = new();
        public List<string> TagNames     { get; set; } = new();

        // --- Metadata ---
        public DateTime DateCreated      { get; set; }
    }
}
