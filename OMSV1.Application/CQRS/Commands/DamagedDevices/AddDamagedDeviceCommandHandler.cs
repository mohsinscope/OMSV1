using MediatR;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedDevices
{
   public class AddDamagedDeviceCommandHandler : IRequestHandler<AddDamagedDeviceCommand, int>
{
    private readonly IGenericRepository<DamagedDevice> _repository;
    private readonly IGenericRepository<DamagedDeviceType> _damagedDeviceTypeRepository;
    private readonly IGenericRepository<DeviceType> _deviceTypeRepository;

    public AddDamagedDeviceCommandHandler(
        IGenericRepository<DamagedDevice> repository,
        IGenericRepository<DamagedDeviceType> damagedDeviceTypeRepository,
        IGenericRepository<DeviceType> deviceTypeRepository)
    {
        _repository = repository;
        _damagedDeviceTypeRepository = damagedDeviceTypeRepository;
        _deviceTypeRepository = deviceTypeRepository;
    }

    public async Task<int> Handle(AddDamagedDeviceCommand request, CancellationToken cancellationToken)
    {
        // Validate DamagedDeviceType
        var damagedDeviceType = await _damagedDeviceTypeRepository.GetByIdAsync(request.DamagedDeviceTypeId);
        if (damagedDeviceType == null)
        {
            throw new Exception($"DamagedDeviceType with ID {request.DamagedDeviceTypeId} not found.");
        }

        // Validate DeviceType
        var deviceType = await _deviceTypeRepository.GetByIdAsync(request.DeviceTypeId);
        if (deviceType == null)
        {
            throw new Exception($"DeviceType with ID {request.DeviceTypeId} not found.");
        }

        var damagedDevice = new DamagedDevice(
            serialNumber: request.SerialNumber,
            date: request.Date,
            damagedDeviceTypeId: request.DamagedDeviceTypeId,
            deviceTypeId: request.DeviceTypeId,
            officeId: request.OfficeId,
            governorateId: request.GovernorateId,
            profileId: request.ProfileId
        );

        var result = await _repository.AddAsync(damagedDevice);
        return result.Id;
    }
}

}
