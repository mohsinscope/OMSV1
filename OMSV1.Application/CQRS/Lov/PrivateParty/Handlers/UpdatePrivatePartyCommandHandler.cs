// Application/Handlers/Documents/UpdatePrivatePartyCommandHandler.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OMSV1.Application.Commands.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Documents
{
    public class UpdatePrivatePartyCommandHandler : IRequestHandler<UpdatePrivatePartyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePrivatePartyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdatePrivatePartyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _unitOfWork.Repository<PrivateParty>()
                    .GetByIdAsync(request.Id);

                if (entity == null)
                    throw new KeyNotFoundException($"PrivateParty with ID {request.Id} not found.");

                // Apply update
                entity.UpdateName(request.Name);

                if (!await _unitOfWork.SaveAsync(cancellationToken))
                    throw new HandlerException("Failed to update the PrivateParty in the database.");

                return true;
            }
            catch (Exception ex) when (!(ex is HandlerException))
            {
                throw new HandlerException("An error occurred while updating the PrivateParty.", ex);
            }
        }
    }
}
