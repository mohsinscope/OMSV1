// Application/Commands/Departments/AddDepartmentsCommand.cs
using MediatR;
using System;

namespace OMSV1.Application.Commands.Department
{
    public class AddDepartmentCommand : IRequest<Guid>
    {
        public string Name { get; }
        public Guid DirectorateId { get; }

        public AddDepartmentCommand(string name, Guid directorateId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            Name = name;

            DirectorateId = directorateId;
            if (directorateId == Guid.Empty)
                throw new ArgumentException("DirectorateId must be provided", nameof(directorateId));
        }
    }
}
