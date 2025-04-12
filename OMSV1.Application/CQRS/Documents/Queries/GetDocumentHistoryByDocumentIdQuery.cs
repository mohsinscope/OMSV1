using MediatR;
using OMSV1.Application.Dtos.Documents;
using System;
using System.Collections.Generic;

namespace OMSV1.Application.Queries.Documents
{
    public class GetDocumentHistoryByDocumentIdQuery : IRequest<List<DocumentHistoryDto>>
    {
        public Guid DocumentId { get; set; }

        public GetDocumentHistoryByDocumentIdQuery(Guid documentId)
        {
            if (documentId == Guid.Empty)
                throw new ArgumentException("DocumentId cannot be empty.", nameof(documentId));

            DocumentId = documentId;
        }
    }
}
