// Application/Handlers/Directorate/UpdateDirectorateCommandHandler.cs
using MediatR;
using OMSV1.Application.Commands.Directorates;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Directorates
{
    public class UpdateDirectorateCommandHandler : IRequestHandler<UpdateDirectorateCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDirectorateCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateDirectorateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _unitOfWork.Repository<Directorate>()
                    .GetByIdAsync(request.Id);

                if (entity == null)
                    throw new KeyNotFoundException($"Directorate with ID {request.Id} not found.");

                // Apply updates
                entity.UpdateName(request.Name);

                if (!await _unitOfWork.SaveAsync(cancellationToken))
                    throw new HandlerException("Failed to update the Directorate in the database.");

                return true;
            }
            catch (Exception ex) when (!(ex is HandlerException))
            {
                throw new HandlerException("An error occurred while updating the Directorate.", ex);
            }
        }
    }
}
