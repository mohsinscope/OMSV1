// Application/Handlers/GeneralDirectorate/UpdateGeneralDirectorateCommandHandler.cs
using MediatR;
using OMSV1.Application.Commands.GeneralDirectorates;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.GeneralDirectorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.GeneralDirectorates
{
    public class UpdateGeneralDirectorateCommandHandler : IRequestHandler<UpdateGeneralDirectorateCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateGeneralDirectorateCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateGeneralDirectorateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _unitOfWork.Repository<GeneralDirectorate>()
                    .GetByIdAsync(request.Id);

                if (entity == null)
                    throw new KeyNotFoundException($"GeneralDirectorate with ID {request.Id} not found.");

                // Apply updates
                entity.UpdateName(request.Name);

                if (!await _unitOfWork.SaveAsync(cancellationToken))
                    throw new HandlerException("Failed to update the GeneralDirectorate in the database.");

                return true;
            }
            catch (Exception ex) when (!(ex is HandlerException))
            {
                throw new HandlerException("An error occurred while updating the GeneralDirectorate.", ex);
            }
        }
    }
}
