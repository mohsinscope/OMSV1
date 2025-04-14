using MediatR;
using OMSV1.Application.Dtos.Documents;

namespace OMSV1.Application.Queries.Documents
{
    public class GetDocumentByDocumentNumberDetailedQuery : IRequest<DocumentDetailedDto>
    {
        public required string DocumentNumber { get; set; }
    }
}
