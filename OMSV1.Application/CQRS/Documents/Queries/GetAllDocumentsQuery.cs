using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Documents
{
    public class GetAllDocumentsQuery : IRequest<PagedList<DocumentDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllDocumentsQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
