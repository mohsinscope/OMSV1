using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.CQRS.Governorates
{
    public class SearchOfficesQueryHandler : IRequestHandler<SearchOfficesQuery, List<OfficeCountDto>>
    {
        private readonly AppDbContext _context;

        public SearchOfficesQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OfficeCountDto>> Handle(SearchOfficesQuery request, CancellationToken cancellationToken)
        {
            if (request.GovernorateId.HasValue)
            {
                // If GovernorateId is provided, return offices for that governorate
                var officeCount = await _context.Offices
                    .Where(o => o.GovernorateId == request.GovernorateId.Value)
                    .GroupBy(o => o.GovernorateId)
                    .Select(g => new OfficeCountDto
                    {
                        GovernorateId = g.Key,
                        NumberOfOffices = g.Count()
                    })
                    .ToListAsync(cancellationToken);

                return officeCount;
            }
            else
            {
                // If GovernorateId is null, return total office count across all governorates
                var totalOffices = await _context.Offices
                    .GroupBy(o => 1)  // Grouping by a constant value to get the total count
                    .Select(g => new OfficeCountDto
                    {
                        GovernorateId = 0, // No specific governorate for total count
                        NumberOfOffices = g.Count()
                    })
                    .ToListAsync(cancellationToken);

                return totalOffices;
            }
        }
    }
}
