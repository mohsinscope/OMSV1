using MediatR;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Application.Commands.Attachments;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Expenses;

namespace OMSV1.Application.Handlers.Attachments
{
    public class AddAttachmentCommandHandler : IRequestHandler<AddAttachmentCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public AddAttachmentCommandHandler(IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        public async Task<string> Handle(AddAttachmentCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
                throw new ArgumentException("No file was uploaded.");

            // Upload the photo using the photo service
            var result = await _photoService.AddPhotoAsync(request.File, request.EntityId, request.EntityType);

            // Check if the entity exists based on the entity type using IUnitOfWork repositories
            switch (request.EntityType)
            {
                case EntityType.DamagedDevice:
                    var damagedDeviceExists = await _unitOfWork.Repository<DamagedDevice>().GetByIdAsync(request.EntityId);
                    if (damagedDeviceExists == null)
                    {
                        throw new ArgumentException($"No damaged device found with ID {request.EntityId}.");
                    }
                    break;

                case EntityType.Lecture:
                    var lectureExists = await _unitOfWork.Repository<Lecture>().GetByIdAsync(request.EntityId);
                    if (lectureExists == null)
                    {
                        throw new ArgumentException($"No lecture found with ID {request.EntityId}.");
                    }
                    break;

                case EntityType.DamagedPassport:
                    var damagedPassportExists = await _unitOfWork.Repository<DamagedPassport>().GetByIdAsync(request.EntityId);
                    if (damagedPassportExists == null)
                    {
                        throw new ArgumentException($"No damaged passport found with ID {request.EntityId}.");
                    }
                    break;
                    case EntityType.Expense:
                    var ExpenseExists = await _unitOfWork.Repository<DailyExpenses>().GetByIdAsync(request.EntityId);
                    if (ExpenseExists == null)
                    {
                        throw new ArgumentException($"No Expense found with ID {request.EntityId}.");
                    }
                    break;

                default:
                    throw new ArgumentException("Unsupported entity type.");
            }

            // Create the attachment entity
            var attachment = new AttachmentCU(
                filePath: result.FilePath,
                entityType: request.EntityType,
                entityId: request.EntityId
            );

            // Add the attachment to the database using the unit of work
            await _unitOfWork.Repository<AttachmentCU>().AddAsync(attachment);

            // Save the changes to the database using unit of work
            if (await _unitOfWork.SaveAsync(cancellationToken))
            {
                return "Added Successfully"; // Success message or DTO could be returned if needed
            }

            throw new Exception("Problem adding the attachment.");
        }
    }
}
