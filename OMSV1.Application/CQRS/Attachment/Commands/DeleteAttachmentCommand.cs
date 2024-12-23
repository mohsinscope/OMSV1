using MediatR;

namespace OMSV1.Application.Commands.Attachment
{
    public class DeleteAttachmentCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteAttachmentCommand(int id)
        {
            Id = id;
        }
    }
}
