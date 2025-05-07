using MediatR;
using System;

namespace OMSV1.Application.Commands.GeneralDirectorates
{
    public class DeleteGeneralDirectorateCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteGeneralDirectorateCommand(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            Id = id;
        }
    }
}
