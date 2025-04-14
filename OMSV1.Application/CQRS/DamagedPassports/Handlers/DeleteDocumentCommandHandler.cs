using MediatR;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Entities.DocumentHistories;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using OMSV1.Application.Commands.Documents;

namespace OMSV1.Application.Handlers.Documents
{
    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDocumentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the document.
                var document = await _unitOfWork.Repository<Document>().GetByIdAsync(request.Id);
                if (document == null)
                    throw new HandlerException($"Document with ID {request.Id} not found.");

                // Retrieve related document history records.
                var documentHistories = _unitOfWork.Repository<DocumentHistory>().Where(dh => dh.DocumentId == document.Id);

                // Delete each history record.
                foreach (var history in documentHistories)
                {
                    await _unitOfWork.Repository<DocumentHistory>().DeleteAsync(history);
                }

                // Delete the document.
                await _unitOfWork.Repository<Document>().DeleteAsync(document);

                // Commit all changes in one transaction.
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                    throw new HandlerException("Failed to save changes to the database during document deletion.");

                return true;
            }
            catch (HandlerException ex)
            {
                throw new HandlerException($"Error in DeleteDocumentCommandHandler: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new HandlerException("An unexpected error occurred while deleting the document.", ex);
            }
        }
    }
}
