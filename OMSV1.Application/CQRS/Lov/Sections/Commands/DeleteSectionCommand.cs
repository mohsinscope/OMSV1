using MediatR;
using System;

namespace OMSV1.Application.Commands.Sections
{
    public class DeleteSectionCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteSectionCommand(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            Id = id;
        }
    }
}
