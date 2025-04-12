using MediatR;
using System;
using OMSV1.Application.Dtos.Projects;

namespace OMSV1.Application.Queries.Projects
{
    public class GetProjectByIdQuery : IRequest<ProjectDto>
    {
        public Guid Id { get; set; }
    }
}
