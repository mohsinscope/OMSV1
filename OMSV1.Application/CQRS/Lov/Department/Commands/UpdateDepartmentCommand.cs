// Application/Commands/Department/UpdateDepartmentCommand.cs
using MediatR;

namespace OMSV1.Application.Commands.Departments
{
    public class UpdateDepartmentCommand : IRequest<bool>
    {
        public Guid Id { get; }
        public string Name { get; }


        public UpdateDepartmentCommand(Guid id, string name)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            Id = id;
            Name = name;
        }
    }
}
