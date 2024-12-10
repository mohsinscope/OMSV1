using MediatR;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedDevices
{
    public class AddDamagedDeviceCommandHandler : IRequestHandler<AddDamagedDeviceCommand, int>
    {
        private readonly IGenericRepository<DamagedDevice> _repository;

        public AddDamagedDeviceCommandHandler(IGenericRepository<DamagedDevice> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(AddDamagedDeviceCommand request, CancellationToken cancellationToken)
        {
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
