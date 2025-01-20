using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OMSV1.Infrastructure.Identity;
using OMSV1.Application.Helpers;
using OMSV1.Application.CQRS.Profiles.Queries;

namespace OMSV1.Application.CQRS.Profiles
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<string>>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public GetAllRolesQueryHandler(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

     public async Task<List<string>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var roles = await _roleManager.Roles
                .Where(role => role.Name != null) // Exclude roles with null names
                .Select(role => role.Name!)
                .ToListAsync(cancellationToken);

            return roles;
        }
        catch (Exception ex)
        {
            throw new HandlerException("An error occurred while retrieving roles.", ex);
        }
    }

    }
}
