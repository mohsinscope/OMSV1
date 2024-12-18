using MediatR;
using OMSV1.Domain.SeedWork;
// Alias the domain profile
using DomainProfile = OMSV1.Domain.Entities.Profiles.Profile;
using OMSV1.Application.Queries.Profiles;
using Microsoft.AspNetCore.Identity;
using OMSV1.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace OMSV1.Application.CQRS.Queries.Profiles
{
    public class GetProfileIdByUserIdQueryHandler(UserManager<ApplicationUser> userManager,IGenericRepository<DomainProfile> repository) : IRequestHandler<GetProfileIdByUserIdQuery, int>
    {


        public async Task<int> Handle(GetProfileIdByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.Users
            .AsQueryable() // Ensure it's treated as IQueryable for EF Core
            .FirstOrDefaultAsync(x => x.NormalizedUserName == request.UserName);
            var profile = await repository.FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
            {
                throw new Exception($"Profile not found for User ID {user.Id}");
            }

            return profile.Id; // Return only the Profile ID
        }
    }
}
