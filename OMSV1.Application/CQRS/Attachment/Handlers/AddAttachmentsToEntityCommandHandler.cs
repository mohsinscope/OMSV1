using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;




namespace OMSV1.Application.Commands.Attachment{
    public class AddAttachmentsToEntityCommandHandler : IRequestHandler<AddAttachmentsToEntityCommand, bool>
{
    private readonly AppDbContext _context;
    private readonly IPhotoService _photoService;

    public AddAttachmentsToEntityCommandHandler(AppDbContext context, IPhotoService photoService)
    {
        _context = context;
        _photoService = photoService;
    }

    public async Task<bool> Handle(AddAttachmentsToEntityCommand request, CancellationToken cancellationToken)
    {
        // Begin a transaction
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // Validate the entity exists
            var entityExists = await _context.Set<Entity>()
    .AnyAsync(e => e.Id == request.EntityId && e.GetType().Name == request.EntityName, cancellationToken);


            if (!entityExists)
            {
                throw new KeyNotFoundException($"No {request.EntityName} found with ID {request.EntityId}.");
            }

            var attachments = new List<AttachmentCU>();

            // Upload each file
            foreach (var file in request.Files)
            {
                var uploadResult = await _photoService.AddPhotoAsync(file);

                if (uploadResult == null)
                {
                    throw new InvalidOperationException("File upload failed.");
                }

                attachments.Add(new AttachmentCU(
                    fileName: file.FileName,
                    filePath: uploadResult.SecureUrl.AbsoluteUri,
                    entityType: Enum.Parse<EntityType>(request.EntityName),
                    entityId: request.EntityId
                ));
            }

            // Add attachments to the database
            _context.AttachmentCUs.AddRange(attachments);
            await _context.SaveChangesAsync(cancellationToken);

            // Commit the transaction
            await transaction.CommitAsync(cancellationToken);

            return true;
        }
        catch
        {
            // Rollback if any error occurs
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

}