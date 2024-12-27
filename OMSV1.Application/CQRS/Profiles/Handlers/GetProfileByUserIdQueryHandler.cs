using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Application.Helpers;
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
            try
            {
                // Fetch the profile with related Governorate and Office
                var profile = await _context.Profiles
                    .Include(p => p.Governorate)
                    .Include(p => p.Office)
                    .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

                if (profile == null)
                {
                    return null; // or throw custom exception, depending on your preference
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
                    GovernorateId = profile.Governorate.Id,
                    OfficeId = profile.Office.Id
                };

                return profileDto;
            }
            catch (Exception ex)
            {
                // Log the error (you can use a logging library like Serilog or NLog here)
                throw new HandlerException("An error occurred while retrieving the profile by user ID.", ex);
            }
        }
    }
}
