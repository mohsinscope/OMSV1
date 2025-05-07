using MediatR;
using System;

namespace OMSV1.Application.Commands.Directorates
{
    public class DeleteDirectorateCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteDirectorateCommand(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            Id = id;
        }
    }
}
