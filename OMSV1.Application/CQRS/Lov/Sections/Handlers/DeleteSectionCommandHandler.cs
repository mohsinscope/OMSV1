using MediatR;
using OMSV1.Application.Commands.Sections;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Sections
{
    public class DeleteSectionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteSectionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the Section entity
                var section = await _unitOfWork.Repository<Section>().GetByIdAsync(request.Id);
                if (section == null)
                {
                    throw new KeyNotFoundException($"section with ID {request.Id} not found.");
                }

                // Delete the entity
                await _unitOfWork.Repository<Section>().DeleteAsync(section);

                // Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to delete the Section from the database.");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while deleting the section.", ex);
            }
        }
    }
}
