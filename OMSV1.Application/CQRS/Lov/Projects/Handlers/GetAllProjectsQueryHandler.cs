using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Projects;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Projects;
using OMSV1.Domain.Entities.Projects;
using OMSV1.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Projects
{
    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, PagedList<ProjectDto>>
    {
        private readonly IGenericRepository<Project> _repository;
        private readonly IMapper _mapper;

        public GetAllProjectsQueryHandler(IGenericRepository<Project> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve all Project entities as an IQueryable
                var queryable = _repository.GetAllAsQueryable();

                // Project to ProjectDto using AutoMapper's ProjectTo extension
                var mappedQuery = queryable.ProjectTo<ProjectDto>(_mapper.ConfigurationProvider);

                // Return the paginated list
                var pagedProjects = await PagedList<ProjectDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return pagedProjects;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An unexpected error occurred while retrieving all projects.", ex);
            }
        }
    }
}
