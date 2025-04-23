// Application/Handlers/Tags/UpdateTagsCommandHandler.cs
using MediatR;
using OMSV1.Application.Commands.Tags;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Tags
{
    public class UpdateTagsCommandHandler : IRequestHandler<UpdateTagsCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTagsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateTagsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _unitOfWork.Repository<Tag>()
                    .GetByIdAsync(request.Id);

                if (entity == null)
                    throw new KeyNotFoundException($"Tags with ID {request.Id} not found.");

                // Apply updates
                entity.UpdateName(request.Name);

                if (!await _unitOfWork.SaveAsync(cancellationToken))
                    throw new HandlerException("Failed to update the Tags in the database.");

                return true;
            }
            catch (Exception ex) when (!(ex is HandlerException))
            {
                throw new HandlerException("An error occurred while updating the Tags.", ex);
            }
        }
    }
}
