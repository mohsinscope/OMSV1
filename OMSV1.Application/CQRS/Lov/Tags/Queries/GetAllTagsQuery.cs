using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Tags
{
    public class GetAllTagsQuery : IRequest<PagedList<TagsDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllTagsQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
