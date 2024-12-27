using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Governorates;
using OMSV1.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Queries.Offices
{
    public class GetGovernoratesWithOfficesForDropdownQueryHandler : IRequestHandler<GetGovernoratesWithOfficesForDropdownQuery, List<GovernorateWithOfficesDropdownDto>>
    {
        private readonly AppDbContext _context;

        public GetGovernoratesWithOfficesForDropdownQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GovernorateWithOfficesDropdownDto>> Handle(GetGovernoratesWithOfficesForDropdownQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch governorates and their related offices
                var governorates = await _context.Governorates
                    .Where(g => !request.GovernorateId.HasValue || g.Id == request.GovernorateId)  // Optional filter for specific GovernorateId
                    .Select(g => new GovernorateWithOfficesDropdownDto
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Offices = g.Offices
                            .Select(o => new OfficeDropdownDto
                            {
                                Id = o.Id,
                                Name = o.Name
                            })
                            .ToList()
                    })
                    .ToListAsync(cancellationToken);

                return governorates;
            }
            catch (Exception ex)
            {
                // Handle the exception and throw a custom HandlerException
                throw new HandlerException("An error occurred while fetching governorates and their offices for dropdown.", ex);
            }
        }
    }
}
