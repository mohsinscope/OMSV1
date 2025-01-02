using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Profiles;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Profiles;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Application.Handlers.Profiles
{
   public class SearchProfilesQueryHandler : IRequestHandler<SearchProfilesQuery, PagedList<ProfileWithUserAndRolesDto>>
{
    private readonly IGenericRepository<OMSV1.Domain.Entities.Profiles.Profile> _repository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public SearchProfilesQueryHandler(
        IGenericRepository<OMSV1.Domain.Entities.Profiles.Profile> repository,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _repository = repository;
        _userManager = userManager;
        _mapper = mapper;
    }

public async Task<PagedList<ProfileWithUserAndRolesDto>> Handle(SearchProfilesQuery request, CancellationToken cancellationToken)
{
    try
    {
        // Fetch profiles based on the provided filters (excluding roles for now)
        var profiles = await _repository.ListAsync(new FilterProfilesSpecification(request.FullName, request.OfficeId, request.GovernorateId));

        var profileWithUserRoles = new List<ProfileWithUserAndRolesDto>();

        foreach (var profile in profiles)
        {
            var user = await _userManager.FindByIdAsync(profile.UserId.ToString());

            if (user != null)
            {
                var roles = (await _userManager.GetRolesAsync(user)).ToList();

                var profileWithRoles = new ProfileWithUserAndRolesDto
                {
                    Id = profile.Id,
                    FullName = profile.FullName,
                    Position = profile.Position.ToString(),
                    GovernorateId = profile.GovernorateId,
                    GovernorateName = profile.Governorate?.Name,
                    OfficeId = profile.OfficeId,
                    OfficeName = profile.Office?.Name,
                    UserId = user.Id,
                    Username = user.UserName,
                    Roles = roles
                };

                profileWithUserRoles.Add(profileWithRoles);
            }
        }

        // Filter profiles by roles if roles are provided in the request
        if (request.Roles != null && request.Roles.Any())
        {
            profileWithUserRoles = profileWithUserRoles
                .Where(p => p.Roles.Any(r => request.Roles.Contains(r)))
                .ToList();
        }

        // Apply pagination
        var paginatedProfiles = profileWithUserRoles
            .Skip((request.PaginationParams.PageNumber - 1) * request.PaginationParams.PageSize)
            .Take(request.PaginationParams.PageSize)
            .ToList();

        return new PagedList<ProfileWithUserAndRolesDto>(
            paginatedProfiles,
            profileWithUserRoles.Count,
            request.PaginationParams.PageNumber,
            request.PaginationParams.PageSize
        );
    }
    catch (Exception ex)
    {
        throw new HandlerException("An error occurred while searching profiles.", ex);
    }
}

}

}

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
