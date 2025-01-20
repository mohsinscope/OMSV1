using MediatR;
using OMSV1.Domain.Entities.DamagedPassport; // For DamagedType entity
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers; // Assuming HandlerException is defined here

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
            try
            {
                // Create the new DamagedType entity from the command
                var damagedType = new DamagedType(request.Name, request.Description?? string.Empty);

                // Use the generic repository to add the new damaged type
                await _unitOfWork.Repository<DamagedType>().AddAsync(damagedType);

                // Commit the transaction (save to the database)
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to save the damaged type to the database.");
                }

                return true; // Return true if the operation was successful
            }
            catch (Exception ex)
            {
                // Log and throw a custom exception if an error occurs
                throw new HandlerException("An error occurred while adding the damaged type.", ex);
            }
        }
    }
}
