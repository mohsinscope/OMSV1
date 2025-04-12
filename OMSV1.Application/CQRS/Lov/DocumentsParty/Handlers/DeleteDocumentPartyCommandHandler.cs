using MediatR;
using OMSV1.Application.Commands.DocumentParties;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.DocumentParties
{
    public class DeleteDocumentPartyCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteDocumentPartyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteDocumentPartyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the DocumentParty entity
                var documentParty = await _unitOfWork.Repository<DocumentParty>().GetByIdAsync(request.Id);
                if (documentParty == null)
                {
                    throw new KeyNotFoundException($"DocumentParty with ID {request.Id} not found.");
                }

                // Delete the entity
                await _unitOfWork.Repository<DocumentParty>().DeleteAsync(documentParty);

                // Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to delete the Document Party from the database.");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while deleting the Document Party.", ex);
            }
        }
    }
}
