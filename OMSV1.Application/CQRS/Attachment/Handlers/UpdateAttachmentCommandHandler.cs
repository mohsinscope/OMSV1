using MediatR;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using OMSV1.Application.Commands.Attachment;
using OMSV1.Domain.Enums;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Application.Specifications;

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
                if (request.NewPhotos == null || request.NewPhotos.Count == 0)
                {
                    throw new ArgumentException("No files were uploaded.");
                }

                // Check if the entity exists based on the entity type using IUnitOfWork repositories
                await ValidateEntityExists(request.EntityId, request.EntityType);

                // Step 1: Get all existing attachments for this entity using specification
                var attachmentSpec = new AttachmentByEntitySpecification(request.EntityId, request.EntityType);
                var existingAttachments = await _unitOfWork.Repository<AttachmentCU>().ListAsync(attachmentSpec);

                // Step 2: Delete old photos from storage and remove from database
                foreach (var attachment in existingAttachments)
                {
                    if (!string.IsNullOrEmpty(attachment.FilePath))
                    {
                        await _photoService.DeletePhotoAsync(attachment.FilePath);
                    }
                    await _unitOfWork.Repository<AttachmentCU>().DeleteAsync(attachment);
                }

                // Step 3: Upload new photos and create new attachment entities
                foreach (var file in request.NewPhotos)
                {
                    if (file != null && file.Length > 0)
                    {
                        // Upload the new photo to storage
                        var uploadResult = await _photoService.AddPhotoAsync(file, request.EntityId, request.EntityType);

                        // Create new attachment entity
                        var newAttachment = new AttachmentCU(
                            filePath: uploadResult.FilePath,
                            entityType: request.EntityType,
                            entityId: request.EntityId
                        );

                        // Add the new attachment to the database
                        await _unitOfWork.Repository<AttachmentCU>().AddAsync(newAttachment);
                    }
                }

                // Step 4: Save all changes
                await _unitOfWork.SaveAsync(cancellationToken);

                return true;
            }
            catch (ArgumentException ex)
            {
                throw new HandlerException("Failed to update attachments: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new HandlerException("An unexpected error occurred while updating attachments.", ex);
            }
        }

        private async Task ValidateEntityExists(Guid entityId, EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.DamagedDevice:
                    var damagedDeviceExists = await _unitOfWork.Repository<DamagedDevice>().GetByIdAsync(entityId);
                    if (damagedDeviceExists == null)
                    {
                        throw new ArgumentException($"No damaged device found with ID {entityId}.");
                    }
                    break;

                case EntityType.Lecture:
                    var lectureExists = await _unitOfWork.Repository<Lecture>().GetByIdAsync(entityId);
                    if (lectureExists == null)
                    {
                        throw new ArgumentException($"No lecture found with ID {entityId}.");
                    }
                    break;

                case EntityType.DamagedPassport:
                    var damagedPassportExists = await _unitOfWork.Repository<DamagedPassport>().GetByIdAsync(entityId);
                    if (damagedPassportExists == null)
                    {
                        throw new ArgumentException($"No damaged passport found with ID {entityId}.");
                    }
                    break;

                case EntityType.Expense:
                    var expenseExists = await _unitOfWork.Repository<DailyExpenses>().GetByIdAsync(entityId);
                    if (expenseExists == null)
                    {
                        throw new ArgumentException($"No expense found with ID {entityId}.");
                    }
                    break;

                default:
                    throw new ArgumentException("Unsupported entity type.");
            }
        }
    }
}