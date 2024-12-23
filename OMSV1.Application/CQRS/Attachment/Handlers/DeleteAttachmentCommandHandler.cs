using MediatR;
using OMSV1.Infrastructure.Persistence;
using OMSV1.Domain.Entities.Attachments;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Commands.Attachment
{
    public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteAttachmentCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
        {
            // Find the attachment by ID
            var attachment = await _context.AttachmentCUs
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (attachment == null)
            {
                // Attachment not found
                return false;
            }

            // Remove the attachment from the database
            _context.AttachmentCUs.Remove(attachment);

            // Save changes to the database
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            return result;
        }
    }
}
