using System;
using System.Collections.Generic;
using MediatR;
using OMSV1.Application.Dtos.GeneralDirectorates;

namespace OMSV1.Application.Queries.GeneralDirectorates
{
    public class GetGeneralDirectoratesByMinistryIdQuery : IRequest<IEnumerable<GeneralDirectorateDto>>
    {
        public Guid MinistryId { get; }

        public GetGeneralDirectoratesByMinistryIdQuery(Guid ministryId)
        {
            if (ministryId == Guid.Empty)
                throw new ArgumentException("MinistryId must be a valid GUID.", nameof(ministryId));

            MinistryId = ministryId;
        }
    }
}
