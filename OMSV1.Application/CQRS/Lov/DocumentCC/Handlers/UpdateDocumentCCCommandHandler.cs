// Application/Handlers/DocumentParties/UpdateDocumentCCCommandHandler.cs
using MediatR;
using OMSV1.Application.Commands.DocumentCC;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Documentcc
{
    public class UpdateDocumentCCCommandHandler : IRequestHandler<UpdateDocumentCCCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDocumentCCCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateDocumentCCCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _unitOfWork.Repository<DocumentCC>()
                    .GetByIdAsync(request.Id);

                if (entity == null)
                    throw new KeyNotFoundException($"DocumentCC with ID {request.Id} not found.");

                // Apply updates
                entity.UpdateName(request.RecipientName);

                if (!await _unitOfWork.SaveAsync(cancellationToken))
                    throw new HandlerException("Failed to update the DocumentCC in the database.");

                return true;
            }
            catch (Exception ex) when (!(ex is HandlerException))
            {
                throw new HandlerException("An error occurred while updating the DocumentCC.", ex);
            }
        }
    }
}
