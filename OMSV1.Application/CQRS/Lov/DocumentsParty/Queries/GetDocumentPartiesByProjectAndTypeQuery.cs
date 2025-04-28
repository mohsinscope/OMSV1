// Application/Queries/DocumentParties/GetDocumentPartiesByProjectAndTypeQuery.cs
using System;
using System.Collections.Generic;
using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Queries.DocumentParties
{
    public class GetDocumentPartiesByProjectAndTypeQuery : IRequest<IEnumerable<DocumentPartyDto>>
    {
        public Guid     ProjectId  { get; }
        public PartyType PartyType  { get; }
        public bool     IsOfficial { get; }

        public GetDocumentPartiesByProjectAndTypeQuery(
            Guid projectId,
            PartyType partyType,
            bool isOfficial)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentException("ProjectId must be a valid GUID.", nameof(projectId));

            ProjectId   = projectId;
            PartyType   = partyType;
            IsOfficial  = isOfficial;
        }
    }
}
