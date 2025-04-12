using MediatR;
using System;

namespace OMSV1.Application.Commands.Projects
{
    public class DeleteProjectCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteProjectCommand(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            Id = id;
        }
    }
}
