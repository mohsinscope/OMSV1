// Application/Handlers/Section/UpdateSectionCommandHandler.cs
using MediatR;
using OMSV1.Application.Commands.Sections;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Sections
{
    public class UpdateSectionCommandHandler : IRequestHandler<UpdateSectionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSectionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _unitOfWork.Repository<Section>()
                    .GetByIdAsync(request.Id);

                if (entity == null)
                    throw new KeyNotFoundException($"Section with ID {request.Id} not found.");

                // Apply updates
                entity.UpdateName(request.Name);

                if (!await _unitOfWork.SaveAsync(cancellationToken))
                    throw new HandlerException("Failed to update the Section in the database.");

                return true;
            }
            catch (Exception ex) when (!(ex is HandlerException))
            {
                throw new HandlerException("An error occurred while updating the Section.", ex);
            }
        }
    }
}
