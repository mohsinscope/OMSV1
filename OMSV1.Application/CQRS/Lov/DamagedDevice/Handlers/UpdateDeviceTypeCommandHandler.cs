using MediatR;
using OMSV1.Infrastructure.Interfaces; // For IUnitOfWork
using OMSV1.Domain.Entities.DamagedDevices; // For DeviceType entity
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.CQRS.Lov.DamagedDevice
{
    public class UpdateDeviceTypeCommandHandler : IRequestHandler<UpdateDeviceTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDeviceTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateDeviceTypeCommand request, CancellationToken cancellationToken)
        {
            // Get the existing device type by ID
            var deviceType = await _unitOfWork.Repository<DeviceType>()
                .GetByIdAsync(request.Id);

            if (deviceType == null)
            {
                return false; // Return false if the device type is not found
            }

            // Update the device type's properties
            deviceType.Update(request.Name, request.Description);

            // Save changes to the database
            await _unitOfWork.SaveAsync(cancellationToken);

            return true; // Return true if the update was successful
        }
    }
}
