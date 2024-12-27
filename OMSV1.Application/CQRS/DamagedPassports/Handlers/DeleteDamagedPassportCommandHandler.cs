using MediatR;
using OMSV1.Application.Commands.DamagedPassports;
using OMSV1.Application.Helpers;
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
            try
            {
                // Use the UnitOfWork to get the repository and find the entity by Id
                var damagedPassport = await _unitOfWork.Repository<DamagedPassport>().GetByIdAsync(request.Id);

                // If the entity does not exist, return false
                if (damagedPassport == null)
                    throw new HandlerException($"Damaged Passport with ID {request.Id} not found.");

                // Perform the delete operation within the UnitOfWork
                await _unitOfWork.Repository<DamagedPassport>().DeleteAsync(damagedPassport);

                // Save the changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                    throw new HandlerException("Failed to save the changes to the database.");

                return true; // Successfully deleted
            }
            catch (HandlerException ex)
            {
                // Handle known exceptions related to the business logic
                throw new HandlerException($"Error in DeleteDamagedPassportCommandHandler: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Catching any unexpected errors and wrapping them in a custom exception
                throw new HandlerException("An unexpected error occurred while deleting the damaged passport.", ex);
            }
        }
    }
}
