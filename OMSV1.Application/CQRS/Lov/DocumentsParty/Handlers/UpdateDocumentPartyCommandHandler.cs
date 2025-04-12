using AutoMapper;
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
    public class UpdateDocumentPartyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateDocumentPartyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> Handle(UpdateDocumentPartyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the DocumentParty entity
                var documentParty = await _unitOfWork.Repository<DocumentParty>().GetByIdAsync(request.Id);
                if (documentParty == null)
                {
                    throw new KeyNotFoundException($"DocumentParty with ID {request.Id} not found.");
                }

                // Update the entity's name using the public method
                documentParty.UpdateName(request.Name);

                // Save changes in the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to update the Document Party in the database.");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while updating the Document Party.", ex);
            }
        }
    }
}
