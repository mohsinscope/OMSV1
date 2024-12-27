using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos;
using OMSV1.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Application.Helpers; // Assuming HandlerException is defined here

namespace OMSV1.Application.Commands.LOV
{
    public class GetAllDamagedDeviceTypesQueryHandler : IRequestHandler<GetAllDamagedDeviceTypesQuery, List<DamagedDeviceTypeDto>>
    {
        private readonly AppDbContext _context;

        public GetAllDamagedDeviceTypesQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DamagedDeviceTypeDto>> Handle(GetAllDamagedDeviceTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch all damaged device types from the database
                var damagedDeviceTypes = await _context.DamagedDeviceTypes
                    .Select(ddt => new DamagedDeviceTypeDto
                    {
                        Id = ddt.Id,
                        Name = ddt.Name,
                        Description = ddt.Description
                    })
                    .ToListAsync(cancellationToken);

                return damagedDeviceTypes;
            }
            catch (Exception ex)
            {
                // If an exception occurs, throw a custom HandlerException
                throw new HandlerException("An error occurred while retrieving the damaged device types.", ex);
            }
        }
    }
}
