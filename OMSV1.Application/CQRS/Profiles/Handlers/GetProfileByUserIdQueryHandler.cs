using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Application.Queries.Profiles;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Application.CQRS.Queries.Profiles
{
    public class GetProfileByUserIdQueryHandler : IRequestHandler<GetProfileByUserIdQuery, ProfileDto>
    {
        private readonly AppDbContext _context;

        public GetProfileByUserIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileDto> Handle(GetProfileByUserIdQuery request, CancellationToken cancellationToken)
        {
            var profile = await _context.Profiles
                .Include(p => p.Governorate)
                .Include(p => p.Office)
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

            if (profile == null)
            {
                return null; // or return a custom error
            }

            // Manually map the entity to the DTO
            var profileDto = new ProfileDto
            {
                ProfileId = profile.Id,
                FullName = profile.FullName,
                Position = profile.Position.ToString(),
                GovernorateName = profile.Governorate.Name,
                OfficeName = profile.Office.Name,
                UserId = request.UserId,
                GovernorateId= profile.Governorate.Id,
                OfficeId=profile.Office.Id
            };

            return profileDto;
        }
    }
}
