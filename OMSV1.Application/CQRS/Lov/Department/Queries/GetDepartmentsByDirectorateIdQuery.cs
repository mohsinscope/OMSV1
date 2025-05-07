using System;
using System.Collections.Generic;
using MediatR;
using OMSV1.Application.Dtos.Departments;

namespace OMSV1.Application.Queries.Departments
{
    public class GetDepartmentsByDirectorateIdQuery : IRequest<IEnumerable<DepartmentDto>>
    {
        public Guid DirectorateId { get; }

        public GetDepartmentsByDirectorateIdQuery(Guid directorateId)
        {
            if (directorateId == Guid.Empty)
                throw new ArgumentException("DirectorateId must be a valid GUID.", nameof(directorateId));

            DirectorateId = directorateId;
        }
    }
}
