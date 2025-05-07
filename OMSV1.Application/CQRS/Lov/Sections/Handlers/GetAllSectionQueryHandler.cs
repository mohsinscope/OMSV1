using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Sections;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Sections;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Sections
{
    public class GetAllSectionsQueryHandler : IRequestHandler<GetAllSectionsQuery, PagedList<SectionDto>>
    {
        private readonly IGenericRepository<Section> _repository;
        private readonly IMapper _mapper;

        public GetAllSectionsQueryHandler(IGenericRepository<Section> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<SectionDto>> Handle(GetAllSectionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get all SectionDto records as an IQueryable
                var queryable = _repository.GetAllAsQueryable();

                // Project the queryable to SectionDto using AutoMapper
                var mappedQuery = queryable.ProjectTo<SectionDto>(_mapper.ConfigurationProvider);

                // Create a paginated list using the provided pagination parameters
                var pagedSections = await PagedList<SectionDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return pagedSections;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An unexpected error occurred while retrieving all Sections.", ex);
            }
        }
    }
}
