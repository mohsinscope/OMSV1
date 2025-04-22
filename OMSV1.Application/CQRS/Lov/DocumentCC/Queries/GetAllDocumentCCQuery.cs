using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.DocumentCC
{
    public class GetAllDocumentCCQuery : IRequest<PagedList<DocumentCCDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllDocumentCCQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
