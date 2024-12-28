using MediatR;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Commands.LOV
{
    public class UpdateDamagedDeviceTypeCommandHandler : IRequestHandler<UpdateDamagedDeviceTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDamagedDeviceTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateDamagedDeviceTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the damaged device type by ID using the generic repository
                var damagedDeviceType = await _unitOfWork.Repository<DamagedDeviceType>()
                    .GetByIdAsync(request.Id);

                if (damagedDeviceType == null)
                {
                    // If not found, return false
                    return false;
                }

                // Update the entity with new values
                damagedDeviceType.Update(request.Name, request.Description);

                // Update the entity in the repository and save changes
                await _unitOfWork.Repository<DamagedDeviceType>().UpdateAsync(damagedDeviceType);

                // Commit the changes to the database
                await _unitOfWork.SaveAsync(cancellationToken);

                return true; // Return true if the update was successful
            }
            catch (Exception ex)
            {
                // Throw a custom exception if an error occurs
                throw new HandlerException("An error occurred while updating the damaged device type.", ex);
            }
        }
    }
}
