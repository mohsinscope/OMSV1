using MediatR;
using OMSV1.Application.Commands.Projects;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Projects;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Projects
{
    public class DeleteProjectCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the Project entity
                var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.Id);
                if (project == null)
                {
                    throw new KeyNotFoundException($"Project with ID {request.Id} was not found.");
                }

                // Delete the Project entity using the repository
                await _unitOfWork.Repository<Project>().DeleteAsync(project);

                // Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to delete the Project from the database.");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while deleting the Project.", ex);
            }
        }
    }
}
