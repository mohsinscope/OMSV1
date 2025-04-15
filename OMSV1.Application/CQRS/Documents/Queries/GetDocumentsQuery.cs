using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Enums;
using System;

namespace OMSV1.Application.Queries.Documents
{
    public class GetDocumentsQuery : IRequest<PagedList<DocumentDto>>
    {
        public string? DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string? Title { get; set; }
        public DocumentType? DocumentType { get; set; }
        public ResponseType? ResponseType { get; set; }
        public Guid? PartyId { get; set; }
        public bool? IsAudited { get; set; }
        public bool? IsReplied { get; set; }
        public bool? IsRequiresReply { get; set; }
        
        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public PaginationParams PaginationParams { get; set; }

        public GetDocumentsQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
