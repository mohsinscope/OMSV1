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
                // Fetch all roles using RoleManager
                var roles = await _roleManager.Roles
                    .Select(role => role.Name)
                    .ToListAsync(cancellationToken);

                return roles; // Return the list of role names
            }
            catch (Exception ex)
            {
                // Log the error (you can use a logging library like Serilog or NLog here)
                throw new HandlerException("An error occurred while retrieving roles.", ex);
            }
        }
    }
}
