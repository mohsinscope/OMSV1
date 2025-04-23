// Application/Handlers/DocumentParties/UpdateDocumentPartyCommandHandler.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OMSV1.Application.Commands.DocumentParties;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DocumentParties
{
    public class UpdateDocumentPartyCommandHandler : IRequestHandler<UpdateDocumentPartyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDocumentPartyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateDocumentPartyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _unitOfWork.Repository<DocumentParty>()
                    .GetByIdAsync(request.Id);

                if (entity == null)
                    throw new KeyNotFoundException($"DocumentParty with ID {request.Id} not found.");

                // Apply updates
                entity.UpdateName(request.Name);
                entity.UpdatePartyType(request.PartyType);
                entity.SetOfficial(request.IsOfficial);
                entity.ChangeProject(request.ProjectId);

                if (!await _unitOfWork.SaveAsync(cancellationToken))
                    throw new HandlerException("Failed to update the DocumentParty in the database.");

                return true;
            }
            catch (Exception ex) when (!(ex is HandlerException))
            {
                throw new HandlerException("An error occurred while updating the DocumentParty.", ex);
            }
        }
    }
}
