using MediatR;
using OMSV1.Application.Commands.Companies;
using OMSV1.Domain.Entities.Companies;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Companies
{
    public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Create a new Company entity
                var company = new Company(request.Name);

                // Add the company to the repository
                await _unitOfWork.Repository<Company>().AddAsync(company);

                // Save changes to the database
                if (await _unitOfWork.SaveAsync(cancellationToken))
                {
                    return company.Id; // Return the newly created Company's ID
                }

                throw new Exception("Failed to add the company.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                throw new HandlerException("An error occurred while adding the company.", ex);
            }
        }
    }
}
