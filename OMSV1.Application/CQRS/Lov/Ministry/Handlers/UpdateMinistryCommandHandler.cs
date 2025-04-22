// Application/Handlers/Ministry/UpdateMinistryCommandHandler.cs
using MediatR;
using OMSV1.Application.Commands.Ministries;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Ministries;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Ministries
{
    public class UpdateMinistryCommandHandler : IRequestHandler<UpdateMinistryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMinistryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateMinistryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _unitOfWork.Repository<Ministry>()
                    .GetByIdAsync(request.Id);

                if (entity == null)
                    throw new KeyNotFoundException($"Ministry with ID {request.Id} not found.");

                // Apply updates
                entity.UpdateName(request.Name);

                if (!await _unitOfWork.SaveAsync(cancellationToken))
                    throw new HandlerException("Failed to update the Ministry in the database.");

                return true;
            }
            catch (Exception ex) when (!(ex is HandlerException))
            {
                throw new HandlerException("An error occurred while updating the Ministry.", ex);
            }
        }
    }
}
