using MediatR;
using OMSV1.Domain.Entities.DamagedPassport; // For DamagedType entity
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers; // Assuming HandlerException is defined here

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
            try
            {
                // Fetch the damaged type from the database
                var damagedType = await _unitOfWork.Repository<DamagedType>()
                    .FirstOrDefaultAsync(dt => dt.Id == request.Id);

                if (damagedType == null)
                {
                    return false; // Return false if the entity does not exist
                }

                // Delete the entity
                await _unitOfWork.Repository<DamagedType>().DeleteAsync(damagedType);

                // Commit the changes to the database and return the result
                return await _unitOfWork.SaveAsync(cancellationToken); // Assuming SaveAsync returns a bool
            }
            catch (Exception ex)
            {
                // Log and throw a custom exception if an error occurs
                throw new HandlerException("An error occurred while deleting the damaged type.", ex);
            }
        }
    }
}
