// using MediatR;
// using Microsoft.AspNetCore.Http;
// using Microsoft.EntityFrameworkCore;
// using OMSV1.Application.Dtos;
// using OMSV1.Domain.Entities.Attachments;
// using OMSV1.Domain.Enums;
// using OMSV1.Infrastructure.Interfaces;
// using OMSV1.Infrastructure.Persistence;
// using System;
// using System.Threading;
// using System.Threading.Tasks;

// namespace OMSV1.Application.Commands.Attachment
// {
//     public class AddAttachmentCommandHandler : IRequestHandler<AddAttachmentCommand, AttachmentDto>
//     {
//         private readonly AppDbContext _context;
//         private readonly IPhotoService _photoService;

//         public AddAttachmentCommandHandler(AppDbContext context, IPhotoService photoService)
//         {
//             _context = context;
//             _photoService = photoService;
//         }

//         public async Task<AttachmentDto> Handle(AddAttachmentCommand request, CancellationToken cancellationToken)
//         {
//             // Validate the incoming file
//             if (request.File == null || request.File.Length == 0)
//             {
//                 throw new ArgumentException("No file was uploaded.");
//             }

//             // Upload the file to Cloudinary
//             var result = await _photoService.AddPhotoAsync(request.File);

//             // Check for the entity type and whether the entity exists
//             if (request.EntityType == EntityType.DamagedDevice)
//             {
//                 var damagedDeviceExists = await _context.DamagedDevices
//                     .FirstOrDefaultAsync(dd => dd.Id == request.EntityId, cancellationToken);

//                 if (damagedDeviceExists == null)
//                 {
//                     throw new ArgumentException($"No damaged device found with ID {request.EntityId}.");
//                 }
//             }
//             else if (request.EntityType == EntityType.DamagedPassport)
//             {
//                 var damagedPassportExists = await _context.DamagedPassports
//                     .FirstOrDefaultAsync(dp => dp.Id == request.EntityId, cancellationToken);

//                 if (damagedPassportExists == null)
//                 {
//                     throw new ArgumentException($"No damaged passport found with ID {request.EntityId}.");
//                 }
//             }

//             // You can extend this block for other entities like Expenses, Devices, etc.

//             // Create the attachment entity
//             var attachmentcu = new AttachmentCU(
//                 fileName: request.File.FileName,
//                 filePath: result.SecureUrl.AbsoluteUri,
//                 entityType: request.EntityType,
//                 entityId: request.EntityId
//             );

//             // Save to the database
//             _context.AttachmentCUs.Add(attachmentcu);

//             if (await _context.SaveChangesAsync(cancellationToken) > 0)
//             {
//                 // Return the DTO
//                 var attachmentDto = new AttachmentDto
//                 {
//                     FileName = attachmentcu.FileName,
//                     FilePath = attachmentcu.FilePath,
//                     EntityId = attachmentcu.EntityId,
//                     EntityType = attachmentcu.EntityType.ToString() // Convert Enum to String for DTO
//                 };

//                 return attachmentDto;
//             }

//             throw new ArgumentException("Problem adding the attachment.");
//         }
//     }
// }
