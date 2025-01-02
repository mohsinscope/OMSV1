using MediatR;

namespace OMSV1.Application.Commands.Attachment
{
    public class DeleteAttachmentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteAttachmentCommand(Guid id)
        {
            Id = id;
        }
    }
}
