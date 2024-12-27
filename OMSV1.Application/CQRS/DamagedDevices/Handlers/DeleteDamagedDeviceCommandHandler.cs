using MediatR;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
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
            try
            {
                // Fetch the damaged device entity
                var damagedDevice = await _unitOfWork.Repository<DamagedDevice>().GetByIdAsync(request.Id);

                if (damagedDevice == null)
                {
                    throw new HandlerException($"Damaged device with ID {request.Id} not found.");
                }

                // Perform the delete operation
                await _unitOfWork.Repository<DamagedDevice>().DeleteAsync(damagedDevice);

                // Save the changes to the database
                if (await _unitOfWork.SaveAsync(cancellationToken))
                {
                    return true; // Successfully deleted
                }

                return false; // Failed to save the changes
            }
            catch (HandlerException ex)
            {
                // Log and rethrow the custom exception
                throw new HandlerException("Error occurred while deleting the damaged device.", ex);
            }
            catch (Exception ex)
            {
                // Catch unexpected errors and rethrow them as HandlerException
                throw new HandlerException("An unexpected error occurred while deleting the damaged device.", ex);
            }
        }
    }
}
