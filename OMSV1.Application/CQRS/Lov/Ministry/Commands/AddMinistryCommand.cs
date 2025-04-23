// Application/Commands/Ministry/AddMinistryCommand.cs
using MediatR;

namespace OMSV1.Application.Commands.Ministries
{
    public class AddMinistryCommand : IRequest<Guid>
    {
        public string Name { get; }

        public AddMinistryCommand(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            Name = name;
        }
    }
}