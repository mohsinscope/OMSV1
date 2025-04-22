// Application/Commands/DocumentParties/AddDocumentCCCommand.cs
using MediatR;

namespace OMSV1.Application.Commands.DocumentCC
{
    public class AddDocumentCCCommand : IRequest<Guid>
    {
        public required string RecipientName { get; set; }
        public AddDocumentCCCommand(string recipientName)
        {
            if (string.IsNullOrWhiteSpace(recipientName))
                throw new ArgumentException("Name cannot be null or empty", nameof(recipientName));
            RecipientName = recipientName;

        }
    }
}
