using MediatR;

namespace OMSV1.Application.Commands.Tags
{
    public class DeleteTagsCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteTagsCommand(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            Id = id;
        }
    }
}
