using MediatR;
using OMSV1.Application.Commands.Directorates;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Directorates
{
    public class DeleteDirectorateCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteDirectorateCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteDirectorateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the Directorate entity
                var Directorate = await _unitOfWork.Repository<Directorate>().GetByIdAsync(request.Id);
                if (Directorate == null)
                {
                    throw new KeyNotFoundException($"Directorate with ID {request.Id} not found.");
                }

                // Delete the entity
                await _unitOfWork.Repository<Directorate>().DeleteAsync(Directorate);

                // Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to delete the Directorate from the database.");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while deleting the Directorate.", ex);
            }
        }
    }
}
