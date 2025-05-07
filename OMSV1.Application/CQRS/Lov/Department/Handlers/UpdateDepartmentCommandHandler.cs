// Application/Handlers/Department/UpdateDepartmentCommandHandler.cs
using MediatR;
using OMSV1.Application.Commands.Departments;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Departments
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDepartmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _unitOfWork.Repository<Department>()
                    .GetByIdAsync(request.Id);

                if (entity == null)
                    throw new KeyNotFoundException($"Department with ID {request.Id} not found.");

                // Apply updates
                entity.UpdateName(request.Name);

                if (!await _unitOfWork.SaveAsync(cancellationToken))
                    throw new HandlerException("Failed to update the Department in the database.");

                return true;
            }
            catch (Exception ex) when (!(ex is HandlerException))
            {
                throw new HandlerException("An error occurred while updating the Department.", ex);
            }
        }
    }
}
