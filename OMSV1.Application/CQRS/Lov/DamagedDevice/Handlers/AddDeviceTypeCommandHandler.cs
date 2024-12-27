using MediatR;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Domain.Entities.DamagedDevices;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.CQRS.Lov.DamagedDevice;
using OMSV1.Application.Helpers; // Assuming HandlerException is defined here

namespace OMSV1.Application.CQRS.Lov.DamagedDevice
{
    public class AddDeviceTypeCommandHandler : IRequestHandler<AddDeviceTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddDeviceTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AddDeviceTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deviceType = new DeviceType(request.Name, request.Description);

                // Use the generic repository to add the new device type
                await _unitOfWork.Repository<DeviceType>().AddAsync(deviceType);
                await _unitOfWork.SaveAsync(cancellationToken); // Commit the transaction

                return true; // Return true if successfully added
            }
            catch (Exception ex)
            {
                // If an exception occurs, throw a custom HandlerException
                throw new HandlerException("An error occurred while adding the device type.", ex);
            }
        }
    }
}
