using MediatR;
using OMSV1.Infrastructure.Interfaces; // For IUnitOfWork
using OMSV1.Domain.Entities.DamagedPassport; // For DamagedType entity
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.CQRS.Lov.DamagedPassport
{
    public class UpdateDamagedTypeCommandHandler : IRequestHandler<UpdateDamagedTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDamagedTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateDamagedTypeCommand request, CancellationToken cancellationToken)
        {
            // Find the existing damaged type
            var damagedType = await _unitOfWork.Repository<DamagedType>()
                .GetByIdAsync(request.Id);

            if (damagedType == null)
            {
                return false; // Return false if the damaged type doesn't exist
            }

            // Update the damaged type's properties
            damagedType.Update(request.Name, request.Description);

            // Save changes
            await _unitOfWork.SaveAsync(cancellationToken);

            return true; // Return true if update is successful
        }
    }
}
