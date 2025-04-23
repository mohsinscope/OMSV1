// Application/Commands/Ministries/UpdateDocumentPartyCommand.cs
using System;
using MediatR;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Commands.Ministries
{
    public class UpdateMinistryCommand : IRequest<bool>
    {
        public Guid Id { get; }
        public string Name { get; }


        public UpdateMinistryCommand(Guid id, string name)
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
