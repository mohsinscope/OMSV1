using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.GeneralDirectorates;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.GeneralDirectories;
using OMSV1.Domain.Entities.GeneralDirectorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.GeneralDirectorates
{
    public class GetAllGeneralDirectoratesQueryHandler : IRequestHandler<GetAllGeneralDirectoratesQuery, PagedList<GeneralDirectorateDto>>
    {
        private readonly IGenericRepository<GeneralDirectorate> _repository;
        private readonly IMapper _mapper;

        public GetAllGeneralDirectoratesQueryHandler(IGenericRepository<GeneralDirectorate> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<GeneralDirectorateDto>> Handle(GetAllGeneralDirectoratesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get all GeneralDirectorateDto records as an IQueryable
                var queryable = _repository.GetAllAsQueryable();

                // Project the queryable to GeneralDirectorateDto using AutoMapper
                var mappedQuery = queryable.ProjectTo<GeneralDirectorateDto>(_mapper.ConfigurationProvider);

                // Create a paginated list using the provided pagination parameters
                var pagedGeneralDirectories = await PagedList<GeneralDirectorateDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return pagedGeneralDirectories;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An unexpected error occurred while retrieving all GeneralDirectories.", ex);
            }
        }
    }
}
