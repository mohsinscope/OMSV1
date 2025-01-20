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
    var query = _officeRepository.GetAllAsQueryable();

    if (request.GovernorateId.HasValue)
    {
        // Filter by GovernorateId
        query = query.Where(o => o.GovernorateId == request.GovernorateId.Value);
    }

    // Group by GovernorateId or use a constant
    var result = await query
        .GroupBy(o => request.GovernorateId.HasValue ? o.GovernorateId : (Guid?)null)
        .Select(g => new OfficeCountDto
        {
            GovernorateId = g.Key ?? Guid.Empty, // Replace null with Guid.Empty
            NumberOfOffices = g.Count()
        })
        .ToListAsync(cancellationToken);

    return result;
}

    }
}
