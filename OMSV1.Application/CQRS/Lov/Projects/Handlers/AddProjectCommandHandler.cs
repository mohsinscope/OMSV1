using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Projects;
using OMSV1.Domain.Entities.Projects;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Handlers.Projects
{
    public class AddProjectCommandHandler : IRequestHandler<AddProjectCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Create a new Project entity using the constructor or mapping if desired
                var project = new Project(request.Name);
                // Alternatively, if using AutoMapper:
                // var project = _mapper.Map<Project>(request);

                // Add the project to the repository
                await _unitOfWork.Repository<Project>().AddAsync(project);

                // Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to save the Project to the database.");
                }

                return project.Id;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while creating the Project.", ex);
            }
        }
    }
}
