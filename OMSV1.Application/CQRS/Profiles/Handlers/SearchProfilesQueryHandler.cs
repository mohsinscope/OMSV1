using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Helpers;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Profiles;
using OMSV1.Application.CQRS.Profiles.Queries;
using OMSV1.Application.Dtos.Profiles;

namespace OMSV1.Application.CQRS.Profiles.Handlers
{
    public class SearchProfilesQueryHandler : IRequestHandler<SearchProfilesQuery, PagedList<ProfileWithUserAndRolesDto>>
    {
        private readonly IGenericRepository<OMSV1.Domain.Entities.Profiles.Profile> _repository;
        private readonly IMapper _mapper;

        public SearchProfilesQueryHandler(IGenericRepository<OMSV1.Domain.Entities.Profiles.Profile> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<ProfileWithUserAndRolesDto>> Handle(SearchProfilesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Create the specification based on the query parameters
                var spec = new FilterProfilesWithUsersSpecification(
                    request.FullName,
                    request.OfficeId,
                    request.GovernorateId
                    
                );

                // Get the queryable list of Profiles entities
                var queryableResult = _repository.ListAsQueryable(spec);

                // Map to Profiles
                var mappedQuery = queryableResult.ProjectTo<ProfileWithUserAndRolesDto>(_mapper.ConfigurationProvider);

                // Create a paginated list of Profiles
                return await PagedList<ProfileWithUserAndRolesDto>.CreateAsync(mappedQuery, request.PaginationParams.PageNumber, request.PaginationParams.PageSize);
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors and throw a custom exception
                throw new HandlerException("An error occurred while retrieving Profiles With Users And Roles.", ex);
            }
        }
    }
}
