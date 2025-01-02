using MediatR;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using OMSV1.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Queries.Profiles;

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
    // Fetch profiles with related Governorate and Office entities
  var profiles = await _unitOfWork.Repository<Profile>()
    .GetAllAsQueryable() // Use GetAllAsQueryable for no specification
    .Include(p => p.Governorate)
    .Include(p => p.Office)
    .ToListAsync(cancellationToken);


    var profileWithUserRoles = new List<ProfileWithUserAndRolesDto>();

    foreach (var profile in profiles)
    {
        // Fetch the user associated with the profile
        var user = await _userManager.FindByIdAsync(profile.UserId.ToString());

        if (user != null)
        {
            // Map the profile and related user data into the DTO
            var profileWithRoles = new ProfileWithUserAndRolesDto
            {
                Id = profile.Id,
                FullName = profile.FullName,
                Position = profile.Position.ToString(),
                GovernorateId = profile.GovernorateId,
                GovernorateName = profile.Governorate?.Name, // Fetch Governorate name
                OfficeId = profile.OfficeId,
                OfficeName = profile.Office?.Name, // Fetch Office name
                UserId = user.Id,
                Username = user.UserName,
                Roles = (await _userManager.GetRolesAsync(user)).ToList() // Fetch roles
            };

            profileWithUserRoles.Add(profileWithRoles);
        }
    }

    return profileWithUserRoles;
}


    }
}
