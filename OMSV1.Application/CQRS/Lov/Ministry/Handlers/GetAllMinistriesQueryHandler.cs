using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Ministries;
using OMSV1.Domain.Entities.Ministries;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Ministries
{
    public class GetAllMinistriesQueryHandler : IRequestHandler<GetAllMinistriesQuery, PagedList<MinistryDto>>
    {
        private readonly IGenericRepository<Ministry> _repository;
        private readonly IMapper _mapper;

        public GetAllMinistriesQueryHandler(IGenericRepository<Ministry> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<MinistryDto>> Handle(GetAllMinistriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get all MinistryDto records as an IQueryable
                var queryable = _repository.GetAllAsQueryable();

                // Project the queryable to MinistryDto using AutoMapper
                var mappedQuery = queryable.ProjectTo<MinistryDto>(_mapper.ConfigurationProvider);

                // Create a paginated list using the provided pagination parameters
                var pagedMinistries = await PagedList<MinistryDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return pagedMinistries;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An unexpected error occurred while retrieving all Ministries.", ex);
            }
        }
    }
}
