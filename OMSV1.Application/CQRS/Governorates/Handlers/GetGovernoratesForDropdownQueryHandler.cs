using MediatR;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace OMSV1.Application.Queries.Governorates
{
    public class GetGovernoratesForDropdownQueryHandler(AppDbContext context) : IRequestHandler<GetGovernoratesForDropdownQuery, List<GovernorateDropdownDto>>
    {
        private readonly AppDbContext _context = context;

        public async Task<List<GovernorateDropdownDto>> Handle(GetGovernoratesForDropdownQuery request, CancellationToken cancellationToken)
        {
            return await _context.Governorates
                .Select(o => new GovernorateDropdownDto
                {
                    Id = o.Id,
                    Name = o.Name
                })
                .ToListAsync(cancellationToken);
        }
    }
}
