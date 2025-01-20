using MediatR;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.CQRS.Lov.DamagedDevice;
using OMSV1.Application.Helpers; // Assuming HandlerException is defined here

namespace OMSV1.Application.Commands.LOV
{
    public class AddDamagedDeviceTypeCommandHandler : IRequestHandler<AddDamagedDeviceTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddDamagedDeviceTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AddDamagedDeviceTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var damagedDeviceType = new DamagedDeviceType(request.Name,  request.Description ?? string.Empty);

                // Use the generic repository to add the new damaged device type
                await _unitOfWork.Repository<DamagedDeviceType>().AddAsync(damagedDeviceType);
                await _unitOfWork.SaveAsync(cancellationToken);  // Commit the transaction

                return true;  // Return true if the entity was added successfully
            }
            catch (Exception ex)
            {
                // Handle the exception and throw a custom HandlerException
                throw new HandlerException("An error occurred while adding the damaged device type.", ex);
            }
        }
    }
}
