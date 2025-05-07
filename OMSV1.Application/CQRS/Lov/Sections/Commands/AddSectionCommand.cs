// Application/Commands/Sections/AddSectionsCommand.cs
using MediatR;
using System;

namespace OMSV1.Application.Commands.Sections
{
    public class AddSectionCommand : IRequest<Guid>
    {
        public string Name { get; }
        public Guid DepartmentId { get; }

        public AddSectionCommand(string name, Guid departmentId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            Name = name;

            DepartmentId = departmentId;
            if (departmentId == Guid.Empty)
                throw new ArgumentException("DepartmentId must be provided", nameof(departmentId));
        }
    }
}
