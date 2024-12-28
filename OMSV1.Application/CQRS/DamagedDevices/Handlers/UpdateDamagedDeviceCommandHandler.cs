using MediatR;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            try
            {
                // Get the damaged device using the repository inside unit of work
                var damagedDevice = await _unitOfWork.Repository<DamagedDevice>().GetByIdAsync(request.Id);

                if (damagedDevice == null)
                {
                    throw new KeyNotFoundException($"Damaged Device with ID {request.Id} not found.");
                }

                // Update the device details
                damagedDevice.UpdateDeviceDetails(
                    request.SerialNumber,
                    request.Date,
                    request.DamagedDeviceTypeId,
                    request.DeviceTypeId,
                    request.OfficeId,
                    request.Note,
                    
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
            catch (KeyNotFoundException ex)
            {
                // Log and handle the exception as needed
                throw new HandlerException("Failed to update the damaged device: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                // Catch any other unexpected exceptions and wrap them in a HandlerException
                throw new HandlerException("An unexpected error occurred while updating the damaged device.", ex);
            }
        }
    }

}
