using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.CQRS.Queries.Permissions;
using OMSV1.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.CQRS.Handlers.Permissions
{
    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, List<string>>
    {
        private readonly AppDbContext _context;

        public GetAllPermissionsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            // Fetch distinct permissions from AppRolePermission and UserPermission and sort them alphabetically
            var rolePermissions = await _context.RolePermissions
                .Select(rp => rp.Permission)
                .Distinct()
                .ToListAsync(cancellationToken);

            var userPermissions = await _context.UserPermissions
                .Select(up => up.Permission)
                .Distinct()
                .ToListAsync(cancellationToken);

            // Combine, remove duplicates, and sort alphabetically
            return rolePermissions
                .Union(userPermissions)
                .Distinct()
                .OrderBy(permission => permission)
                .ToList();
        }
    }
}
