using MediatR;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Application.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Infrastructure.Interfaces;

namespace OMSV1.Application.Commands.Attachment
{
    public class UpdateAttachmentCommandHandler : IRequestHandler<UpdateAttachmentCommand, AttachmentDto>
    {
        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;

        public UpdateAttachmentCommandHandler(AppDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        public async Task<AttachmentDto> Handle(UpdateAttachmentCommand request, CancellationToken cancellationToken)
        {
            // Find the attachment by ID
            var attachment = await _context.AttachmentCUs
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (attachment == null)
            {
                // Attachment not found, return null
                return null;
            }

            // If a new file is uploaded, update the file and file path
            if (request.File != null)
            {
                // Upload the new file and get the URL
                var result = await _photoService.AddPhotoAsync(request.File);

                // Update the attachment's file path
                attachment.UpdateFilePath(result.SecureUrl.AbsoluteUri);
            }

            // Update other properties
            attachment.Update(request.FileName, attachment.FilePath, request.EntityType, request.EntityId);

            // Save changes to the database
            var resultSave = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (resultSave)
            {
                // Return the updated AttachmentDto
                return new AttachmentDto
                {
                    Id = attachment.Id,
                    FileName = attachment.FileName,
                    FilePath = attachment.FilePath,
                    EntityType = attachment.EntityType,
                    EntityId = attachment.EntityId
                };
            }

            return null;
        }
    }
}
