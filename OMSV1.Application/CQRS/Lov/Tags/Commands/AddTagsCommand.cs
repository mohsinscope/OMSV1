// Application/Commands/Tag/AddTagCommand.cs
using MediatR;

namespace OMSV1.Application.Commands.Tags
{
    public class AddTagsCommand : IRequest<Guid>
    {
        public string Name { get; }

        public AddTagsCommand(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            Name = name;
        }
    }
}