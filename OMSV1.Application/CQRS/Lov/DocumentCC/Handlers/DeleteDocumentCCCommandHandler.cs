using MediatR;
using OMSV1.Application.Commands.DocumentCC;
using OMSV1.Application.Commands.DocumentParties;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Documentcc
{
    public class DeleteDocumentCCCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteDocumentCCCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteDocumentCCCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the DocumentCC entity
                var documentCC = await _unitOfWork.Repository<DocumentCC>().GetByIdAsync(request.Id);
                if (documentCC == null)
                {
                    throw new KeyNotFoundException($"DocumentCC with ID {request.Id} not found.");
                }

                // Delete the entity
                await _unitOfWork.Repository<DocumentCC>().DeleteAsync(documentCC);

                // Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to delete the Document CC from the database.");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while deleting the Document CC.", ex);
            }
        }
    }
}
