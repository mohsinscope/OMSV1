using MediatR;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers; // Assuming HandlerException is defined here

namespace OMSV1.Application.Commands.LOV
{
    public class DeleteDamagedDeviceTypeCommandHandler : IRequestHandler<DeleteDamagedDeviceTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDamagedDeviceTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDamagedDeviceTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Use the IUnitOfWork to find the damaged device type by ID
                var damagedDeviceType = await _unitOfWork.Repository<DamagedDeviceType>()
                    .GetByIdAsync(request.Id);

                if (damagedDeviceType == null)
                    return false; // Return false if the device type does not exist

                // Use the repository to delete the entity
                await _unitOfWork.Repository<DamagedDeviceType>().DeleteAsync(damagedDeviceType);

                // Commit the changes to the database
                await _unitOfWork.SaveAsync(cancellationToken);

                return true; // Return true if deletion was successful
            }
            catch (Exception ex)
            {
                // If an exception occurs, throw a custom HandlerException
                throw new HandlerException("An error occurred while deleting the damaged device type.", ex);
            }
        }
    }
}
