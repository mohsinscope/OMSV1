using MediatR;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.CQRS.Queries.Profiles
{
    public class GetProfilesWithUsersAndRolesQueryHandler : IRequestHandler<GetProfilesWithUsersAndRolesQuery, List<ProfileWithUserAndRolesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetProfilesWithUsersAndRolesQueryHandler(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<List<ProfileWithUserAndRolesDto>> Handle(GetProfilesWithUsersAndRolesQuery request, CancellationToken cancellationToken)
        {
            // Fetch profiles using IUnitOfWork repository pattern (no cancellation token here)
            var profiles = await _unitOfWork.Repository<Profile>().GetAllAsync();

            var profileWithUserRoles = new List<ProfileWithUserAndRolesDto>();

            foreach (var profile in profiles)
            {
                var user = await _userManager.FindByIdAsync(profile.UserId.ToString());

                if (user != null)
                {
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
                        Roles = (await _userManager.GetRolesAsync(user)).ToList() // Convert IList<string> to List<string>
                    };

                    profileWithUserRoles.Add(profileWithRoles);
                }
            }

            return profileWithUserRoles;
        }
    }
}
