using MediatR;
using System;

namespace OMSV1.Application.Commands.Departments
{
    public class DeleteDepartmentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteDepartmentCommand(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            Id = id;
        }
    }
}
