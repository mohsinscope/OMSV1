using MediatR;
using System;

namespace OMSV1.Application.Commands.Ministries
{
    public class DeleteMinistryCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteMinistryCommand(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            Id = id;
        }
    }
}
