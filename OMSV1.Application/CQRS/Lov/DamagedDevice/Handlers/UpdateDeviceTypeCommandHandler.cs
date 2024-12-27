using MediatR;
using OMSV1.Infrastructure.Interfaces; // For IUnitOfWork
using OMSV1.Domain.Entities.DamagedDevices; // For DeviceType entity
using System;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers; // Assuming HandlerException is defined here

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
            try
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
            catch (Exception ex)
            {
                // Log and throw a custom exception if something goes wrong
                throw new HandlerException("An error occurred while updating the device type.", ex);
            }
        }
    }
}
