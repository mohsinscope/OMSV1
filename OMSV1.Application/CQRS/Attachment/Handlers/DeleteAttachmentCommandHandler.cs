using MediatR;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Commands.Attachment;

namespace OMSV1.Application.Handlers.Attachments
{
    public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAttachmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
        {
            // Fetch the attachment by ID using the repository inside the unit of work
            var attachment = await _unitOfWork.Repository<AttachmentCU>().GetByIdAsync(request.Id);

            if (attachment == null)
            {
                // Attachment not found
                return false;
            }

            // Delete the attachment asynchronously from the repository
            await _unitOfWork.Repository<AttachmentCU>().DeleteAsync(attachment);

            // Save changes to the database using the unit of work
            var result = await _unitOfWork.SaveAsync(cancellationToken);

            return result;
        }
    }
}
