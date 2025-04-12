using MediatR;
using OMSV1.Application.Dtos.Projects;
using OMSV1.Domain.Entities.Projects;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Queries.Projects
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProjectByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the Project entity using its ID
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.Id);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {request.Id} was not found.");
            }

            // Map the retrieved entity to the DTO
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name
            };
        }
    }
}
