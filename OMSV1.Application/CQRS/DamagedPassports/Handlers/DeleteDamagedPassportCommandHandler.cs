using MediatR;
using OMSV1.Application.Commands.DamagedPassports;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DamagedPassports
{
    public class DeleteDamagedPassportCommandHandler : IRequestHandler<DeleteDamagedPassportCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDamagedPassportCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDamagedPassportCommand request, CancellationToken cancellationToken)
        {
            // Use the UnitOfWork to get the repository and find the entity by Id
            var damagedPassport = await _unitOfWork.Repository<DamagedPassport>().GetByIdAsync(request.Id);

            // If the entity does not exist, return false
            if (damagedPassport == null)
                return false;

            // Perform the delete operation within the UnitOfWork
            await _unitOfWork.Repository<DamagedPassport>().DeleteAsync(damagedPassport);

            // Save the changes to the database
            if (await _unitOfWork.SaveAsync(cancellationToken))
            {
                return true; // Successfully deleted
            }

            return false; // Failed to save the changes
        }
    }
}
