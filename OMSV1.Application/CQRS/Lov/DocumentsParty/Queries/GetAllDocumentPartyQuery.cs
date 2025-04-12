using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.DocumentParties
{
    public class GetAllDocumentPartiesQuery : IRequest<PagedList<DocumentPartyDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllDocumentPartiesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
