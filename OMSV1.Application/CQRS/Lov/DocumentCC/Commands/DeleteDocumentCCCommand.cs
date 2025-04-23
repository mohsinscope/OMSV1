using MediatR;
using System;

namespace OMSV1.Application.Commands.DocumentCC
{
    public class DeleteDocumentCCCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteDocumentCCCommand(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            Id = id;
        }
    }
}
