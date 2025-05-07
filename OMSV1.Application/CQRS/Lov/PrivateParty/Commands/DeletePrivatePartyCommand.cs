// Application/Commands/Documents/DeletePrivatePartyCommand.cs
using MediatR;
using System;

namespace OMSV1.Application.Commands.Documents
{
    public class DeletePrivatePartyCommand : IRequest<bool>
    {
        public Guid Id { get; }

        public DeletePrivatePartyCommand(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            Id = id;
        }
    }
}
