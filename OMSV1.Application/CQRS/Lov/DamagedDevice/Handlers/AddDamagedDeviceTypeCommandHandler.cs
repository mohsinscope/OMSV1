using MediatR;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Domain.Entities.DamagedDevices;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.CQRS.Lov.DamagedDevice;

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
            var damagedDeviceType = new DamagedDeviceType(request.Name, request.Description);

            // Use the generic repository to add the new damaged device type
            await _unitOfWork.Repository<DamagedDeviceType>().AddAsync(damagedDeviceType);
            await _unitOfWork.SaveAsync(cancellationToken);  // Commit the transaction

            return true;  // Return true if the entity was added successfully
        }
    }
}
