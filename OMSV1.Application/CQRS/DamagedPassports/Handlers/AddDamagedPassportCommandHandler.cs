using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.DamagedPassports;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Interfaces;

namespace OMSV1.Application.Handlers.DamagedPassports
{
    public class AddDamagedPassportWithAttachmentCommandHandler : IRequestHandler<AddDamagedPassportWithAttachmentCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public AddDamagedPassportWithAttachmentCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }

public async Task<Guid> Handle(AddDamagedPassportWithAttachmentCommand request, CancellationToken cancellationToken)
{
    try
    {
        // 1. Check for duplicate passport number
        var existingPassport = await _unitOfWork.Repository<DamagedPassport>()
            .FirstOrDefaultAsync(dp => dp.PassportNumber == request.PassportNumber);
        if (existingPassport != null)
        {
            throw new DuplicatePassportException($"A damaged passport with number {request.PassportNumber} already exists.");
        }

        // 2. Verify the office belongs to the provided governorate
        var office = await _unitOfWork.Repository<Office>()
            .FirstOrDefaultAsync(o => o.Id == request.OfficeId && o.GovernorateId == request.GovernorateId);
        if (office == null)
        {
            throw new HandlerException(
                $"Office ID {request.OfficeId} does not belong to Governorate ID {request.GovernorateId}."
            );
        }

        // 3. Map the command to the damaged passport entity and set the date kind
        var damagedPassport = _mapper.Map<DamagedPassport>(request);
        damagedPassport.UpdateDate(DateTime.SpecifyKind(request.Date, DateTimeKind.Utc));

        // 4. Add the damaged passport to the repository
        await _unitOfWork.Repository<DamagedPassport>().AddAsync(damagedPassport);

        // 5. Validate all files before processing attachments
        if (request.File == null || request.File.Count == 0)
        {
            throw new ArgumentException("No files were uploaded for the attachments.");
        }

        // 6. For each file, upload it, then create and add the attachment
        foreach (var file in request.File)
        {
            if (file?.Length > 0)
            {
                var photoResult = await _photoService.AddPhotoAsync(file, damagedPassport.Id, EntityType.DamagedPassport);

                var attachment = new AttachmentCU(
                    filePath: photoResult.FilePath,
                    entityType: EntityType.DamagedPassport,
                    entityId: damagedPassport.Id
                );

                await _unitOfWork.Repository<AttachmentCU>().AddAsync(attachment);
            }
        }

        // 7. Save all changes in one transaction
        if (!await _unitOfWork.SaveAsync(cancellationToken))
        {
            throw new HandlerException(
                "Failed to save the damaged passport and attachments to the database."
            );
        }

        // Return the newly created damaged passport's ID
        return damagedPassport.Id;
    }
    catch (DuplicatePassportException)
    {
        throw; // let the controller handle it
    }
    catch (HandlerException ex)
    {
        throw new HandlerException(
            $"Error in AddDamagedPassportWithAttachmentCommandHandler: {ex.Message}", ex
        );
    }
    catch (Exception ex)
    {
        throw new HandlerException(
            "An unexpected error occurred while adding the damaged passport with attachment.",
            ex
        );
    }
}

    }
}
