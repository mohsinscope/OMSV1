using MediatR;
using OMSV1.Application.Dtos.Directorates;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Directories
{
    public class GetAllDirectoratesQuery : IRequest<PagedList<DirectorateDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllDirectoratesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
