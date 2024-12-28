using MediatR;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Application.Dtos.Attachments;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using OMSV1.Application.Commands.Attachment;
using OMSV1.Application.Commands.Attachments;

namespace OMSV1.Application.Handlers.Attachments
{
    public class AddAttachmentCommandHandler : IRequestHandler<AddAttachmentCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;
        private readonly AppDbContext _appDbContext;

        public AddAttachmentCommandHandler(IUnitOfWork unitOfWork, IPhotoService photoService, AppDbContext appDbContext)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _appDbContext = appDbContext;
        }

        public async Task<string> Handle(AddAttachmentCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
                throw new ArgumentException("No file was uploaded.");

            // Upload the photo using the photo service
            var result = await _photoService.AddPhotoAsync(request.File, request.EntityId, request.EntityType);

            // Check if the entity exists based on the entity type
            switch (request.EntityType)
            {
                case EntityType.DamagedDevice:
                    var damagedDeviceExists = await _appDbContext.DamagedDevices
                        .FirstOrDefaultAsync(dd => dd.Id == request.EntityId, cancellationToken);
                    if (damagedDeviceExists == null)
                    {
                        throw new ArgumentException($"No damaged device found with ID {request.EntityId}.");
                    }
                    break;

                case EntityType.Lecture:
                    var lectureExists = await _appDbContext.Lectures
                        .FirstOrDefaultAsync(dd => dd.Id == request.EntityId, cancellationToken);
                    if (lectureExists == null)
                    {
                        throw new ArgumentException($"No lecture found with ID {request.EntityId}.");
                    }
                    break;

                case EntityType.DamagedPassport:
                    var damagedPassportExists = await _appDbContext.DamagedPassports
                        .FirstOrDefaultAsync(dp => dp.Id == request.EntityId, cancellationToken);
                    if (damagedPassportExists == null)
                    {
                        throw new ArgumentException($"No damaged passport found with ID {request.EntityId}.");
                    }
                    break;

                default:
                    throw new ArgumentException("Unsupported entity type.");
            }

            // Create the attachment entity
            var attachment = new AttachmentCU(
                fileName: result.FileName,
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
