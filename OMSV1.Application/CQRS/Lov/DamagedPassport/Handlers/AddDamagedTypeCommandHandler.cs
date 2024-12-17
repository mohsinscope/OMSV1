using MediatR;
using OMSV1.Infrastructure.Interfaces; // For IUnitOfWork
using OMSV1.Domain.Entities.DamagedPassport; // For DamagedType entity
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Commands.LOV
{
    public class AddDamagedTypeCommandHandler : IRequestHandler<AddDamagedTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddDamagedTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AddDamagedTypeCommand request, CancellationToken cancellationToken)
        {
            var damagedType = new DamagedType(request.Name, request.Description);

            // Use the generic repository to add the new damaged type
            await _unitOfWork.Repository<DamagedType>().AddAsync(damagedType);
            await _unitOfWork.SaveAsync(cancellationToken);  // Commit the transaction

            return true;
        }
    }
}
