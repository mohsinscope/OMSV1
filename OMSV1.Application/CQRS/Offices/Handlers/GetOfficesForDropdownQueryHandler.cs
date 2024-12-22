using MediatR;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Queries.Offices
{
    public class GetOfficesForDropdownQueryHandler : IRequestHandler<GetOfficesForDropdownQuery, List<OfficeDropdownDto>>
    {
        private readonly AppDbContext _context;

        public GetOfficesForDropdownQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OfficeDropdownDto>> Handle(GetOfficesForDropdownQuery request, CancellationToken cancellationToken)
        {
            return await _context.Offices
                .Select(o => new OfficeDropdownDto
                {
                    Id = o.Id,
                    Name = o.Name
                })
                .ToListAsync(cancellationToken);
        }
    }
}
