using MediatR;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedDevices
{
    public class UpdateDamagedDeviceCommandHandler : IRequestHandler<UpdateDamagedDeviceCommand, bool>
    {
        private readonly IGenericRepository<DamagedDevice> _repository;

        public UpdateDamagedDeviceCommandHandler(IGenericRepository<DamagedDevice> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateDamagedDeviceCommand request, CancellationToken cancellationToken)
        {
            var damagedDevice = await _repository.GetByIdAsync(request.Id);

            if (damagedDevice == null) return false; // Device not found

            damagedDevice.UpdateDeviceDetails(
                request.SerialNumber,
                request.Date,
                request.DamagedDeviceTypeId,
                request.DeviceTypeId,
                request.OfficeId,
                request.GovernorateId,
                request.ProfileId
            );

            await _repository.UpdateAsync(damagedDevice);
            return true;
        }
    }
}
