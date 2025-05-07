// Application/Commands/Directorates/AddDirectorateCommand.cs
using MediatR;
using System;

namespace OMSV1.Application.Commands.Directorates
{
    public class AddDirectorateCommand : IRequest<Guid>
    {
        public string Name { get; }
        public Guid GeneralDirectorateId { get; }

        public AddDirectorateCommand(string name, Guid generalDirectorateId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            Name = name;

            GeneralDirectorateId = generalDirectorateId;
            if (generalDirectorateId == Guid.Empty)
                throw new ArgumentException("GeneralDirectorateId must be provided", nameof(generalDirectorateId));
        }
    }
}
