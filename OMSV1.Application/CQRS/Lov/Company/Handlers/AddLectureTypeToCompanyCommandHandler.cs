using MediatR;
using OMSV1.Application.Commands.LectureTypes;
using OMSV1.Domain.Entities.Companies;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.LectureTypes
{
    public class AddLectureTypeToCompanyCommandHandler : IRequestHandler<AddLectureTypeToCompanyCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddLectureTypeToCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddLectureTypeToCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate if the company exists
                var company = await _unitOfWork.Repository<Company>().FirstOrDefaultAsync(c => c.Id == request.CompanyId);
                if (company == null)
                {
                    throw new Exception($"Company ID {request.CompanyId} does not exist.");
                }

                // Create a new LectureType entity
                var lectureType = new LectureType(request.Name, request.CompanyId);

                // Add the lecture type to the repository
                await _unitOfWork.Repository<LectureType>().AddAsync(lectureType);

                // Save changes to the database
                if (await _unitOfWork.SaveAsync(cancellationToken))
                {
                    return lectureType.Id; // Return the newly created LectureType's ID
                }

                throw new Exception("Failed to add the lecture type.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                throw new HandlerException("An error occurred while adding the lecture type to the company.", ex);
            }
        }
    }
}
