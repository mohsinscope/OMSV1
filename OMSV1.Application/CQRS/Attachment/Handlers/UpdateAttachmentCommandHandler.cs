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

            // If a new file is uploaded, handle the replacement process
            if (request.File != null)
            {
                // Delete the old image from storage (only if the file path is available)
                if (!string.IsNullOrEmpty(attachment.FilePath))
                {
                    // Extract the public ID of the old image from the FilePath (assuming the file path contains the public ID)
                    var oldFilePublicId = GetFilePublicIdFromUrl(attachment.FilePath);

                    // Remove the old image from storage
                    await _photoService.DeletePhotoAsync(oldFilePublicId); // You will need to implement this method in your IPhotoService
                }

                // Upload the new file and get the URL
                var result = await _photoService.AddPhotoAsync(request.File,attachment.EntityId,attachment.EntityType);

                // Update the attachment's file path with the new URL
                // attachment.UpdateFilePath(result.SecureUrl.AbsoluteUri);
                attachment.UpdateFilePath(result.FilePath);

            }

            // Update other properties of the attachment
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

        // Helper method to extract the public ID from the URL
        private string GetFilePublicIdFromUrl(string fileUrl)
        {
            // Assuming the file URL is from Cloudinary, you would extract the public ID like so:
            var uri = new Uri(fileUrl);
            var segments = uri.AbsolutePath.Split('/');
            var publicId = segments[segments.Length - 1].Split('.')[0]; // Remove file extension
            return publicId;
        }
    }
}
