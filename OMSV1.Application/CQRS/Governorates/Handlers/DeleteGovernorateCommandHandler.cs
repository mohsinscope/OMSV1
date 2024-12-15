using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Governorates
{
    public class DeleteGovernorateCommandHandler : IRequestHandler<DeleteGovernorateCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteGovernorateCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteGovernorateCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the governorate by ID using the unit of work
            var governorate = await _unitOfWork.Repository<Governorate>().GetByIdAsync(request.Id);

            if (governorate == null) return false; // Governorate not found

            // Remove the governorate using the repository inside unit of work
            await _unitOfWork.Repository<Governorate>().DeleteAsync(governorate);

            // Save changes via the unit of work
            if (await _unitOfWork.SaveAsync(cancellationToken))
            {
                return true;
            }

            return false;
        }
    }
}
