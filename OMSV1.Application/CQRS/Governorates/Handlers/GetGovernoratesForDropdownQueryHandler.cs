using MediatR;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Governorates
{
    public class GetGovernoratesForDropdownQueryHandler : IRequestHandler<GetGovernoratesForDropdownQuery, List<GovernorateDropdownDto>>
    {
        private readonly AppDbContext _context;

        public GetGovernoratesForDropdownQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GovernorateDropdownDto>> Handle(GetGovernoratesForDropdownQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch the list of Governorates and map it to GovernorateDropdownDto
                return await _context.Governorates
                    .Select(o => new GovernorateDropdownDto
                    {
                        Id = o.Id,
                        Name = o.Name
                    })
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Handle the exception and throw a custom HandlerException
                throw new HandlerException("An error occurred while fetching governorates for dropdown.", ex);
            }
        }
    }
}
