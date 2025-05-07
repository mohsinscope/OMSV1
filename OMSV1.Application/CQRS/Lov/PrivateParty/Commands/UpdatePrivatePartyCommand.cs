// Application/Commands/Documents/UpdatePrivatePartyCommand.cs
using MediatR;
using System;

namespace OMSV1.Application.Commands.Documents
{
    public class UpdatePrivatePartyCommand : IRequest<bool>
    {
        public Guid Id { get; }
        public string Name { get; }

        public UpdatePrivatePartyCommand(Guid id, string name)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            Id   = id;
            Name = name;
        }
    }
}
