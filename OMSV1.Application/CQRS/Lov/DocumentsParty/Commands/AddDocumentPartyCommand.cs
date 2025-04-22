// Application/Commands/DocumentParties/AddDocumentPartyCommand.cs
using System;
using MediatR;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Commands.DocumentParties
{
    public class AddDocumentPartyCommand : IRequest<Guid>
    {
        public string Name { get; }
        public PartyType PartyType { get; }
        public bool IsOfficial { get; }
        public Guid ProjectId { get; }

        public AddDocumentPartyCommand(string name, PartyType partyType, bool isOfficial, Guid projectId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            if (projectId == Guid.Empty)
                throw new ArgumentException("ProjectId must be a valid GUID.", nameof(projectId));

            Name = name;
            PartyType = partyType;
            IsOfficial = isOfficial;
            ProjectId = projectId;
        }
    }
}
