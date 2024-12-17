using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.LOV;
using OMSV1.Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.CQRS.Lov.DamagedDevice
{
    public class GetDeviceTypesQueryByIdHandler : IRequestHandler<GetDeviceTypesQueryById, DeviceTypeDto>
    {
        private readonly AppDbContext _context;

        public GetDeviceTypesQueryByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DeviceTypeDto> Handle(GetDeviceTypesQueryById request, CancellationToken cancellationToken)
        {
            // Fetch the device type by Id from the database
            var deviceType = await _context.DeviceTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(dt => dt.Id == request.Id, cancellationToken);

            if (deviceType == null)
            {
                // If not found, return null or handle the error as per your design (e.g., throw an exception)
                return null;
            }

            // Map the entity to DTO
            var deviceTypeDto = new DeviceTypeDto
            {
                Id = deviceType.Id,
                Name = deviceType.Name,
                Description = deviceType.Description
            };

            return deviceTypeDto;
        }
    }
}
