using MediatR;
using OMSV1.Infrastructure.Interfaces; // For IUnitOfWork
using OMSV1.Domain.Entities.DamagedPassport; // For DamagedType entity
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Commands.LOV
{
    public class DeleteDamagedTypeCommandHandler : IRequestHandler<DeleteDamagedTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDamagedTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDamagedTypeCommand request, CancellationToken cancellationToken)
        {
            // Find the damaged type by ID using FirstOrDefaultAsync
            var damagedType = await _unitOfWork.Repository<DamagedType>()
                .FirstOrDefaultAsync(dt => dt.Id == request.Id);

            if (damagedType == null)
            {
                return false; // Return false if the damaged type is not found
            }

            // Delete the damaged type from the repository
            await _unitOfWork.Repository<DamagedType>().DeleteAsync(damagedType);

            // Commit the changes to the database
            await _unitOfWork.SaveAsync(cancellationToken);

            return true; // Return true if deletion was successful
        }
    }
}
