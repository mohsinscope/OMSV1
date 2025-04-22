using MediatR;
using OMSV1.Application.Commands.Ministries;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Ministries;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Ministries
{
    public class DeleteMinistryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteMinistryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteMinistryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the Ministry entity
                var ministry = await _unitOfWork.Repository<Ministry>().GetByIdAsync(request.Id);
                if (ministry == null)
                {
                    throw new KeyNotFoundException($"Ministry with ID {request.Id} not found.");
                }

                // Delete the entity
                await _unitOfWork.Repository<Ministry>().DeleteAsync(ministry);

                // Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to delete the Ministry from the database.");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while deleting the Ministry.", ex);
            }
        }
    }
}
