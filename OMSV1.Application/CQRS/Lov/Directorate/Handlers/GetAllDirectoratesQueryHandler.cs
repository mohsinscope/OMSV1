using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Directorates;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Directories;
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Directorates
{
    public class GetAllDirectoratesQueryHandler : IRequestHandler<GetAllDirectoratesQuery, PagedList<DirectorateDto>>
    {
        private readonly IGenericRepository<Directorate> _repository;
        private readonly IMapper _mapper;

        public GetAllDirectoratesQueryHandler(IGenericRepository<Directorate> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<DirectorateDto>> Handle(GetAllDirectoratesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get all DirectorateDto records as an IQueryable
                var queryable = _repository.GetAllAsQueryable();

                // Project the queryable to DirectorateDto using AutoMapper
                var mappedQuery = queryable.ProjectTo<DirectorateDto>(_mapper.ConfigurationProvider);

                // Create a paginated list using the provided pagination parameters
                var pagedDirectories = await PagedList<DirectorateDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return pagedDirectories;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An unexpected error occurred while retrieving all Directories.", ex);
            }
        }
    }
}
