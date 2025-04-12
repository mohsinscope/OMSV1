using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Projects;
using OMSV1.Domain.Entities.Projects;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Projects
{
    public class UpdateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateProjectCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the existing Project entity
                var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.Id);
                if (project == null)
                {
                    throw new KeyNotFoundException($"Project with ID {request.Id} was not found.");
                }

                // Update the project's name using a domain method (or direct assignment if appropriate)
                project.UpdateName(request.Name);
                // Alternatively, if you choose direct assignment (not recommended if you want domain encapsulation):
                // project.Name = request.Name;

                // Save changes in the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to update the Project in the database.");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while updating the Project.", ex);
            }
        }
    }
}
