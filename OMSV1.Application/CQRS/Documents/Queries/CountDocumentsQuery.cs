// Application/Queries/Documents/CountDocumentsQuery.cs
using System;
using System.Collections.Generic;
using MediatR;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Queries.Documents
{
    public class CountDocumentsQuery : IRequest<int>
    {
        // existing filters...
        public string?   DocumentNumber   { get; set; }
        public DateTime? DocumentDate     { get; set; }
        public string?   Title            { get; set; }
        public string?   Subject          { get; set; }
        public DocumentType? DocumentType { get; set; }
        public ResponseType? ResponseType { get; set; }

        public bool? IsRequiresReply      { get; set; }
        public bool? IsReplied            { get; set; }
        public bool? IsAudited            { get; set; }
        public bool? IsUrgent             { get; set; }
        public bool? IsImportant          { get; set; }
        public bool? IsNeeded             { get; set; }

        public string? Notes              { get; set; }

        public Guid? ProjectId            { get; set; }
        public Guid? PrivatePartyId       { get; set; }
        public Guid? ParentDocumentId     { get; set; }
        public Guid? ProfileId            { get; set; }

        // --- NEW hierarchy filters ---
        public Guid? SectionId            { get; set; }
        public Guid? DepartmentId         { get; set; }
        public Guid? DirectorateId        { get; set; }
        public Guid? GeneralDirectorateId { get; set; }
        public Guid? MinistryId           { get; set; }

        // link filters
        public List<Guid>? CcIds          { get; set; }
        public List<Guid>? TagIds         { get; set; }
    }
}
