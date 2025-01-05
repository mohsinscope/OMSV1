using MediatR;
using OMSV1.Application.Commands.Companies;
using OMSV1.Domain.Entities.Companies;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Handlers.Companies
{
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch the company entity
                var company = await _unitOfWork.Repository<Company>().GetByIdAsync(request.Id);

                if (company == null)
                {
                    throw new HandlerException($"Company with ID {request.Id} not found.");
                }

                // Perform the delete operation
                await _unitOfWork.Repository<Company>().DeleteAsync(company);

                // Save the changes to the database
                if (await _unitOfWork.SaveAsync(cancellationToken))
                {
                    return true; // Successfully deleted
                }

                return false; // Failed to save the changes
            }
            catch (HandlerException ex)
            {
                // Log and rethrow the custom exception
                throw new HandlerException("Error occurred while deleting the company.", ex);
            }
            catch (Exception ex)
            {
                // Catch unexpected errors and rethrow them as HandlerException
                throw new HandlerException("An unexpected error occurred while deleting the company.", ex);
            }
        }
    }
}
