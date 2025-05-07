using MediatR;
using OMSV1.Application.Commands.GeneralDirectorates;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.GeneralDirectorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.GeneralDirectorates
{
    public class DeleteGeneralDirectorateCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteGeneralDirectorateCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteGeneralDirectorateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the GeneralDirectorate entity
                var generalDirectorate = await _unitOfWork.Repository<GeneralDirectorate>().GetByIdAsync(request.Id);
                if (generalDirectorate == null)
                {
                    throw new KeyNotFoundException($"generalDirectorate with ID {request.Id} not found.");
                }

                // Delete the entity
                await _unitOfWork.Repository<GeneralDirectorate>().DeleteAsync(generalDirectorate);

                // Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to delete the generalDirectorate from the database.");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while deleting the GeneralDirectorate.", ex);
            }
        }
    }
}
