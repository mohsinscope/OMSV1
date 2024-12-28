using MediatR;
using OMSV1.Application.Dtos.LOV;
using OMSV1.Infrastructure.Interfaces; // For IUnitOfWork
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Application.Helpers;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.CQRS.Lov.DamagedDevice
{
    public class GetDeviceTypesQueryByIdHandler : IRequestHandler<GetDeviceTypesQueryById, DeviceTypeDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDeviceTypesQueryByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DeviceTypeDto> Handle(GetDeviceTypesQueryById request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch the device type by Id using the unit of work and repository
                var deviceType = await _unitOfWork.Repository<DeviceType>().GetByIdAsync(request.Id);

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
            catch (Exception ex)
            {
                // Throw a custom exception with the error message and inner exception
                throw new HandlerException("An error occurred while retrieving the device type by ID.", ex);
            }
        }
    }
}
