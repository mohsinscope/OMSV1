using MediatR;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers; // optional if you have a custom exception type
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Commands.Documents
{
    public class MarkDocumentAsAuditedCommandHandler : IRequestHandler<MarkDocumentAsAuditedCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MarkDocumentAsAuditedCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(MarkDocumentAsAuditedCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the document by its ID.
            var document = await _unitOfWork.Repository<Document>().GetByIdAsync(request.DocumentId);
            if (document == null)
                throw new KeyNotFoundException($"Document with ID {request.DocumentId} was not found.");

            // Mark the document as audited.
            document.MarkAsAudited();

            // Update the repository and await the task.
            await _unitOfWork.Repository<Document>().UpdateAsync(document);

            // Save the changes.
            if (await _unitOfWork.SaveAsync(cancellationToken))
                return true;

            throw new Exception("Failed to mark the document as audited.");
        }
    }
}
