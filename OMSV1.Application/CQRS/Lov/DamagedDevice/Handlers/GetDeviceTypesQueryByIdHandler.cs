using MediatR;
using OMSV1.Application.Dtos.LOV;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Application.Helpers;
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
        // Fetch the device type by ID
        var deviceType = await _unitOfWork.Repository<DeviceType>().GetByIdAsync(request.Id);

        if (deviceType == null)
        {
            throw new KeyNotFoundException($"DeviceType with ID {request.Id} not found.");
        }

        // Map the entity to DTO
        return new DeviceTypeDto
        {
            Id = deviceType.Id,
            Name = deviceType.Name,
            Description = deviceType.Description
        };
    }
    catch (Exception ex)
    {
        throw new HandlerException("An error occurred while retrieving the device type by ID.", ex);
    }
}

    }
}
