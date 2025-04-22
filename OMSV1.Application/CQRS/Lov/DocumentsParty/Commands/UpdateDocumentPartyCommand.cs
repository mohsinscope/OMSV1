// Application/Commands/DocumentParties/UpdateDocumentPartyCommand.cs
using System;
using MediatR;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Commands.DocumentParties
{
    public class UpdateDocumentPartyCommand : IRequest<bool>
    {
        public Guid Id { get; }
        public string Name { get; }
        public PartyType PartyType { get; }
        public bool IsOfficial { get; }
        public Guid ProjectId { get; }

        public UpdateDocumentPartyCommand(Guid id, string name, PartyType partyType, bool isOfficial, Guid projectId)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            if (projectId == Guid.Empty)
                throw new ArgumentException("ProjectId must be a valid GUID.", nameof(projectId));

            Id = id;
            Name = name;
            PartyType = partyType;
            IsOfficial = isOfficial;
            ProjectId = projectId;
        }
    }
}
