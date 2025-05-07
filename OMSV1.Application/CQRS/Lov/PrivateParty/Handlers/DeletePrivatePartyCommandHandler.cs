// Application/Handlers/Documents/DeletePrivatePartyCommandHandler.cs
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
    public class DeletePrivatePartyCommandHandler : IRequestHandler<DeletePrivatePartyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePrivatePartyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeletePrivatePartyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1) Retrieve the entity
                var party = await _unitOfWork.Repository<PrivateParty>()
                    .GetByIdAsync(request.Id);

                if (party == null)
                    throw new KeyNotFoundException($"PrivateParty with ID {request.Id} not found.");

                // 2) Delete it
                await _unitOfWork.Repository<PrivateParty>().DeleteAsync(party);

                // 3) Save changes
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                    throw new HandlerException("Failed to delete the PrivateParty from the database.");

                return true;
            }
            catch (Exception ex) when (!(ex is HandlerException))
            {
                throw new HandlerException("An error occurred while deleting the PrivateParty.", ex);
            }
        }
    }
}
