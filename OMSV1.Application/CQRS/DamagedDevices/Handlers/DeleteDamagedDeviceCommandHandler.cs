using MediatR;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.DamagedDevices
{
    public class DeleteDamagedDeviceCommandHandler : IRequestHandler<DeleteDamagedDeviceCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDamagedDeviceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDamagedDeviceCommand request, CancellationToken cancellationToken)
        {
            // Fetch the damaged device entity
            var damagedDevice = await _unitOfWork.Repository<DamagedDevice>().GetByIdAsync(request.Id);

            if (damagedDevice == null)
                return false; // If not found, return false

            // Perform the delete operation
            await _unitOfWork.Repository<DamagedDevice>().DeleteAsync(damagedDevice);

            // Save the changes to the database
            if (await _unitOfWork.SaveAsync(cancellationToken))
            {
                return true; // Successfully deleted
            }

            return false; // Failed to save the changes
        }
    }
}
