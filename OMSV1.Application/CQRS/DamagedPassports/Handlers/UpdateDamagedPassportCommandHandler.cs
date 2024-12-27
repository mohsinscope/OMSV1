using MediatR;
using OMSV1.Application.Commands.DamagedPassports;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.SeedWork;
using AutoMapper;
using OMSV1.Application.Helpers;  // Assuming you have a custom exception handler

namespace OMSV1.Application.Handlers.DamagedPassports
{
    public class UpdateDamagedPassportCommandHandler : IRequestHandler<UpdateDamagedPassportCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDamagedPassportCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateDamagedPassportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Use the unit of work to get the repository and find the damaged passport by Id
                var damagedPassport = await _unitOfWork.Repository<DamagedPassport>().GetByIdAsync(request.Id);

                // If the entity does not exist, return false
                if (damagedPassport == null)
                    return false;

                // Map the updated values from the command to the existing entity
                _mapper.Map(request, damagedPassport);

                // Update the entity within the UnitOfWork
                await _unitOfWork.Repository<DamagedPassport>().UpdateAsync(damagedPassport);

                // Save the changes to the database within the transaction
                if (await _unitOfWork.SaveAsync(cancellationToken))
                {
                    return true; // Successfully updated
                }

                return false; // Failed to save the changes
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur and throw a custom exception
                throw new HandlerException("An error occurred while updating the damaged passport.", ex);
            }
        }
    }
}
