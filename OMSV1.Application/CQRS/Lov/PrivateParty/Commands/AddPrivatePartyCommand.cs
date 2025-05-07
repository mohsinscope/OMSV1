// Application/Commands/Documents/AddPrivatePartyCommand.cs
using MediatR;
using System;

namespace OMSV1.Application.Commands.Documents
{
    public class AddPrivatePartyCommand : IRequest<Guid>
    {
        public string Name { get; }

        public AddPrivatePartyCommand(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            Name = name;
        }
    }
}
