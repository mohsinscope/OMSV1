using MediatR;
using OMSV1.Application.Dtos.Documents;
using System;

namespace OMSV1.Application.Queries.Documents
{
    public class GetDocumentByIdDetailedQuery : IRequest<DocumentDetailedDto>
    {
        public Guid Id { get; set; }
    }
}
