using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OMSV1.Application.CQRS.Profiles.Queries;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Application.Helpers;
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
                var profiles = await _repository.ListAsync(new FilterProfilesSpecification(request.FullName ??string.Empty, request.OfficeId, request.GovernorateId));

                var profileWithUserRoles = new List<ProfileWithUserAndRolesDto>();

                foreach (var profile in profiles)
                {
                    var user = await _userManager.FindByIdAsync(profile.UserId.ToString());

                    if (user != null)
                    {
                        var roles = (await _userManager.GetRolesAsync(user))?.ToList() ?? new List<string>(); // Handle null roles

                        var profileWithRoles = new ProfileWithUserAndRolesDto
                        {
                            Id = profile.Id,
                            FullName = profile.FullName ?? "Unknown", // Handle null FullName
                            Position = profile.Position.ToString(),
                            GovernorateId = profile.GovernorateId,
                            GovernorateName = profile.Governorate?.Name ?? "Unknown Governorate", // Handle null Governorate
                            OfficeId = profile.OfficeId,
                            OfficeName = profile.Office?.Name ?? "Unknown Office", // Handle null Office
                            UserId = user.Id,
                            Username = user.UserName ?? "Unknown User", // Handle null Username
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
