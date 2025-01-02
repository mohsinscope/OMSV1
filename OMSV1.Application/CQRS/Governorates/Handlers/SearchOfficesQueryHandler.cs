using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.CQRS.Governorates
{
    public class SearchOfficesQueryHandler : IRequestHandler<SearchOfficesQuery, List<OfficeCountDto>>
    {
        private readonly IGenericRepository<Office> _officeRepository;

        public SearchOfficesQueryHandler(IGenericRepository<Office> officeRepository)
        {
            _officeRepository = officeRepository;
        }

        public async Task<List<OfficeCountDto>> Handle(SearchOfficesQuery request, CancellationToken cancellationToken)
        {
            if (request.GovernorateId.HasValue)
            {
                // If GovernorateId is provided, return offices for that governorate
                var officeCount = await _officeRepository.GetAllAsQueryable()
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
                var totalOffices = await _officeRepository.GetAllAsQueryable()
                    .GroupBy(o => 1)  // Grouping by a constant value to get the total count
                    .Select(g => new OfficeCountDto
                    {
                        GovernorateId = Guid.Empty, // Use Guid.Empty for no specific governorate
                        NumberOfOffices = g.Count()
                    })
                    .ToListAsync(cancellationToken);

                return totalOffices;
            }
        }
    }
}
