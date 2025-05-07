using MediatR;
using OMSV1.Application.Commands.Departments;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Departments
{
    public class DeleteDepartmentCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteDepartmentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the Department entity
                var department = await _unitOfWork.Repository<Department>().GetByIdAsync(request.Id);
                if (department == null)
                {
                    throw new KeyNotFoundException($"Department with ID {request.Id} not found.");
                }

                // Delete the entity
                await _unitOfWork.Repository<Department>().DeleteAsync(department);

                // Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to delete the Department from the database.");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while deleting the Department.", ex);
            }
        }
    }
}
