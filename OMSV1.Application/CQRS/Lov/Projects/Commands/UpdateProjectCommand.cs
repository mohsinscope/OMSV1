using MediatR;
using System;

namespace OMSV1.Application.Commands.Projects
{
    public class UpdateProjectCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public UpdateProjectCommand(Guid id, string name)
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
