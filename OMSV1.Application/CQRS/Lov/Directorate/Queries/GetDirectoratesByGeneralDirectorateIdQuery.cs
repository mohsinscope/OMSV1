// Application/Queries/Directorates/GetDirectoratesByGeneralDirectorateIdQuery.cs
using System;
using System.Collections.Generic;
using MediatR;
using OMSV1.Application.Dtos.Directorates;

namespace OMSV1.Application.Queries.Directorates
{
    public class GetDirectoratesByGeneralDirectorateIdQuery : IRequest<IEnumerable<DirectorateDto>>
    {
        public Guid GeneralDirectorateId { get; }

        public GetDirectoratesByGeneralDirectorateIdQuery(Guid generalDirectorateId)
        {
            if (generalDirectorateId == Guid.Empty)
                throw new ArgumentException("GeneralDirectorateId must be a valid GUID.", nameof(generalDirectorateId));

            GeneralDirectorateId = generalDirectorateId;
        }
    }
}
