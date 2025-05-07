// Application/Queries/Sections/GetSectionsByDepartmentIdQuery.cs
using System;
using System.Collections.Generic;
using MediatR;
using OMSV1.Application.Dtos.Sections;

namespace OMSV1.Application.Queries.Sections
{
    public class GetSectionsByDepartmentIdQuery : IRequest<IEnumerable<SectionDto>>
    {
        public Guid DepartmentId { get; }

        public GetSectionsByDepartmentIdQuery(Guid departmentId)
        {
            if (departmentId == Guid.Empty)
                throw new ArgumentException("DepartmentId must be a valid GUID.", nameof(departmentId));

            DepartmentId = departmentId;
        }
    }
}
