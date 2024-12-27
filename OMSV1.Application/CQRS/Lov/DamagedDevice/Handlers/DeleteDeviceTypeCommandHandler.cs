using MediatR;
using OMSV1.Infrastructure.Interfaces; // For IUnitOfWork
using OMSV1.Domain.Entities.DamagedDevices;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Commands.LOV;
using OMSV1.Application.Helpers; // Assuming HandlerException is defined here

namespace OMSV1.Application.CQRS.Lov.DamagedDevice
{
    public class DeleteDeviceTypeCommandHandler : IRequestHandler<DeleteDeviceTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDeviceTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDeviceTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Use GetByIdAsync to find the device type by ID
                var deviceType = await _unitOfWork.Repository<DeviceType>()
                    .GetByIdAsync(request.Id);

                if (deviceType == null)
                {
                    return false; // Return false if the device type does not exist
                }

                // Remove the device type from the repository
                await _unitOfWork.Repository<DeviceType>().DeleteAsync(deviceType);

                // Save changes to the database
                await _unitOfWork.SaveAsync(cancellationToken);

                return true; // Return true if deletion was successful
            }
            catch (Exception ex)
            {
                // If an exception occurs, throw a custom HandlerException
                throw new HandlerException("An error occurred while deleting the device type.", ex);
            }
        }
    }
}
