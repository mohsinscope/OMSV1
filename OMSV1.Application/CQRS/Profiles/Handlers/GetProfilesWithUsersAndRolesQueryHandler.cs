using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Application.CQRS.Queries.Profiles;

public class GetProfilesWithUsersAndRolesQueryHandler : IRequestHandler<GetProfilesWithUsersAndRolesQuery, List<ProfileWithUserAndRolesDto>>
{
    private readonly AppDbContext _context;

    public GetProfilesWithUsersAndRolesQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProfileWithUserAndRolesDto>> Handle(GetProfilesWithUsersAndRolesQuery request, CancellationToken cancellationToken)
    {
        var profiles = await _context.Profiles
            .Include(p => p.Governorate) 
            .Include(p => p.Office)     
            .Join(_context.Users,
                profile => profile.UserId,
                user => user.Id,
                (profile, user) => new ProfileWithUserAndRolesDto
                {
                    Id=profile.Id,
                    FullName = profile.FullName,
                    Position = profile.Position.ToString(),
                    GovernorateId = profile.GovernorateId,
                    GovernorateName = profile.Governorate!.Name,
                    OfficeId = profile.OfficeId,
                    OfficeName = profile.Office.Name,
                    UserId = user.Id,
                    Username = user.UserName,
                    Roles = user.UserRoles.Select(r => r.Role.Name).ToList()
                })
            .OrderBy(x => x.Username)
            .ToListAsync(cancellationToken);

        return profiles;
    }
}