// Application/Commands/GeneralDirectorates/AddGeneralDirectorateCommand.cs
using MediatR;
using System;

namespace OMSV1.Application.Commands.GeneralDirectorates
{
    public class AddGeneralDirectorateCommand : IRequest<Guid>
    {
        public string Name { get; }
        public Guid MinistryId { get; }

        public AddGeneralDirectorateCommand(string name, Guid ministryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            Name = name;

            MinistryId = ministryId;
            if (ministryId == Guid.Empty)
                throw new ArgumentException("MinistryId must be provided", nameof(ministryId));
        }
    }
}
