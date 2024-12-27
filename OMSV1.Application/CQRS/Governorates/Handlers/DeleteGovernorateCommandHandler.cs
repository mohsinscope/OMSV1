using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;  // Assuming HandlerException is defined in this namespace

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
            try
            {
                // Retrieve the governorate by ID using the unit of work
                var governorate = await _unitOfWork.Repository<Governorate>().GetByIdAsync(request.Id);

                if (governorate == null) return false; // Governorate not found

                // Remove the governorate using the repository inside unit of work
                await _unitOfWork.Repository<Governorate>().DeleteAsync(governorate);

                // Save changes via the unit of work
                if (await _unitOfWork.SaveAsync(cancellationToken))
                {
                    return true;  // Successfully deleted
                }

                return false;  // Failed to save changes
            }
            catch (Exception ex)
            {
                // Catch and throw a custom exception for better error reporting
                throw new HandlerException("An error occurred while deleting the governorate.", ex);
            }
        }
    }
}
