using MediatR;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedDevices
{
    public class UpdateDamagedDeviceCommandHandler : IRequestHandler<UpdateDamagedDeviceCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDamagedDeviceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateDamagedDeviceCommand request, CancellationToken cancellationToken)
        {
            // Get the damaged device using the repository inside unit of work
            var damagedDevice = await _unitOfWork.Repository<DamagedDevice>().GetByIdAsync(request.Id);

            if (damagedDevice == null) return false; // Device not found

            // Update the device details
            damagedDevice.UpdateDeviceDetails(
                request.SerialNumber,
                request.Date,
                request.DamagedDeviceTypeId,
                request.DeviceTypeId,
                request.OfficeId,
                request.GovernorateId,
                request.ProfileId
            );

            // Update the entity using the repository inside unit of work
            await _unitOfWork.Repository<DamagedDevice>().UpdateAsync(damagedDevice);

            // Save the changes using unit of work
            if (await _unitOfWork.SaveAsync(cancellationToken))
            {
                return true;
            }

            return false;
        }
    }
}
