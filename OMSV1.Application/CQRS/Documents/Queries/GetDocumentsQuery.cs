// Application/Queries/Documents/GetDocumentsQuery.cs
using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Enums;
using System;

namespace OMSV1.Application.Queries.Documents
{
    public class GetDocumentsQuery : IRequest<PagedList<DocumentDto>>
    {
        // --- existing filters ---
        public string? DocumentNumber   { get; set; }
        public DateTime? DocumentDate   { get; set; }
        public string? Title            { get; set; }
        public string? Subject          { get; set; }
        public DocumentType? DocumentType   { get; set; }
        public ResponseType? ResponseType   { get; set; }
        public bool? IsRequiresReply    { get; set; }
        public bool? IsReplied          { get; set; }
        public bool? IsAudited          { get; set; }
        public bool? IsUrgent           { get; set; }
        public bool? IsImportant        { get; set; }
        public bool? IsNeeded           { get; set; }
        public string? Notes            { get; set; }
        public Guid? ProjectId          { get; set; }
        public Guid? PrivatePartyId     { get; set; }
        public Guid? ParentDocumentId   { get; set; }
        public Guid? ProfileId          { get; set; }

        // --- NEW: hierarchy filters ---
        public Guid? SectionId          { get; set; }
        public Guid? DepartmentId       { get; set; }
        public Guid? DirectorateId      { get; set; }
        public Guid? GeneralDirectorateId { get; set; }
        public Guid? MinistryId         { get; set; }

        // --- pagination ---
        public int PageNumber { get; set; } = 1;
        public int PageSize   { get; set; } = 10;
    }
}
