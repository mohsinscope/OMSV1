using MediatR;
using System;

namespace OMSV1.Application.Commands.Projects
{
    public class AddProjectCommand : IRequest<Guid>
    {
        public string Name { get; set; }

        public AddProjectCommand(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            
            Name = name;
        }
    }
}
