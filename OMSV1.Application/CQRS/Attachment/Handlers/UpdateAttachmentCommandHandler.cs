using MediatR;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Application.Dtos.Attachments;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Application.Helpers;
using System.Net.Mail;
using OMSV1.Application.Commands.Attachment;

namespace OMSV1.Application.Handlers.Attachments
{
    public class UpdateAttachmentCommandHandler : IRequestHandler<UpdateAttachmentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public UpdateAttachmentCommandHandler(IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        public async Task<bool> Handle(UpdateAttachmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the attachment entity from the database
                var attachment = await _unitOfWork.Repository<AttachmentCU>().GetByIdAsync(request.AttachmentId);
                if (attachment == null)
                {
                    throw new KeyNotFoundException($"Attachment with ID {request.AttachmentId} not found.");
                }

                // Step 1: Delete the old photo from the local storage (if it exists)
                if (!string.IsNullOrEmpty(attachment.FilePath))
                {
                    await _photoService.DeletePhotoAsync(attachment.FilePath);
                }

                // Step 2: Upload the new photo to the local storage
                var uploadResult = await _photoService.AddPhotoAsync(request.NewPhoto, request.EntityId, request.EntityType);

                // Step 3: Update the attachment entity with the new photo details
                attachment.Update(uploadResult.FileName, uploadResult.FilePath);

                // Step 4: Update the attachment in the database asynchronously
                await _unitOfWork.Repository<AttachmentCU>().UpdateAsync(attachment);

                // Step 5: Save the changes asynchronously
                await _unitOfWork.SaveAsync(cancellationToken);

                return true;
            }
            catch (KeyNotFoundException ex)
            {
                // Handle the case where the attachment is not found
                throw new HandlerException("Failed to update the attachment: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                // Catch other unexpected errors and throw a custom exception
                throw new HandlerException("An unexpected error occurred while updating the attachment.", ex);
            }
        }


    }
}
