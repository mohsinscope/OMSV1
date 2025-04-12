using MediatR;
using System;

namespace OMSV1.Application.Commands.DocumentParties
{
    public class DeleteDocumentPartyCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteDocumentPartyCommand(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            Id = id;
        }
    }
}
