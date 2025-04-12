using MediatR;
using OMSV1.Application.Dtos.Projects;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Projects
{
    public class GetAllProjectsQuery : IRequest<PagedList<ProjectDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllProjectsQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
