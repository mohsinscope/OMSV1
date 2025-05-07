using MediatR;
using OMSV1.Application.Dtos.Sections;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Sections
{
    public class GetAllSectionsQuery : IRequest<PagedList<SectionDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllSectionsQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
