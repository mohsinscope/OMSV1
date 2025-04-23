// Application/Commands/DocumentParties/UpdateDocumentCCCommand.cs
using MediatR;

namespace OMSV1.Application.Commands.DocumentCC
{
    public class UpdateDocumentCCCommand : IRequest<bool>
    {
        public Guid Id { get; }
        public required string RecipientName { get; set; }
        public UpdateDocumentCCCommand(Guid id, string recipientName)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            if (string.IsNullOrWhiteSpace(recipientName))
                throw new ArgumentException("Name cannot be null or empty.", nameof(recipientName));

            Id = id;
            RecipientName = recipientName;

        }
    }
}
