using MediatR;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Application.Dtos;
using OMSV1.Domain.Entities.DamagedDevices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Helpers;  // Assuming HandlerException is defined here

namespace OMSV1.Application.Queries.LOV
{
    public class GetDamagedDeviceTypeQueryHandler : IRequestHandler<GetDamagedDeviceTypeQuery, DamagedDeviceTypeDto>
    {
        private readonly AppDbContext _context;

        public GetDamagedDeviceTypeQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DamagedDeviceTypeDto> Handle(GetDamagedDeviceTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch the damaged device type by ID
                var damagedDeviceType = await _context.DamagedDeviceTypes
                    .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

                if (damagedDeviceType == null)
                    return null; // Return null if the device type is not found

                // Map and return the DTO
                return new DamagedDeviceTypeDto
                {
                    Id = damagedDeviceType.Id,
                    Name = damagedDeviceType.Name,
                    Description = damagedDeviceType.Description
                };
            }
            catch (Exception ex)
            {
                // If an error occurs, throw a custom HandlerException with the error message and inner exception
                throw new HandlerException("An error occurred while retrieving the damaged device type.", ex);
            }
        }
    }
}
