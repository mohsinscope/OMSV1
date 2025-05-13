using MediatR;
using OMSV1.Application.Dtos.Documents;
using System;

namespace OMSV1.Application.Queries.Documents
{
    public class GetDocumentByDocumentNumberDetailedQuery : IRequest<DocumentDetailedDto>
    {
        public string DocumentNumber { get; }
        public int Depth { get; }

        public GetDocumentByDocumentNumberDetailedQuery(string documentNumber, int depth)
        {
            DocumentNumber = documentNumber;
            Depth = depth;
        }
    }
}