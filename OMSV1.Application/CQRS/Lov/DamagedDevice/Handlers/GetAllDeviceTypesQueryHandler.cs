using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.LOV;
using OMSV1.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.CQRS.Lov.DamagedDevice
{
    public class GetAllDeviceTypesQueryHandler : IRequestHandler<GetAllDeviceTypesQuery, IEnumerable<DeviceTypeDto>>
    {
        private readonly AppDbContext _context;

        public GetAllDeviceTypesQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeviceTypeDto>> Handle(GetAllDeviceTypesQuery request, CancellationToken cancellationToken)
        {
            // Fetch all DeviceTypes from the database
            var deviceTypes = await _context.DeviceTypes
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // Map the entities to DTOs
            var deviceTypeDtos = deviceTypes.Select(dt => new DeviceTypeDto
            {
                Id = dt.Id,
                Name = dt.Name,
                Description = dt.Description
            }).ToList();

            return deviceTypeDtos;
        }
    }
}
