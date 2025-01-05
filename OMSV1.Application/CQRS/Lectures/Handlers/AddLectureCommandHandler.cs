using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Lectures;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.Entities.Companies;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Lectures
{
    public class AddLectureCommandHandler : IRequestHandler<AddLectureCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddLectureCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddLectureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Step 1: Validate if the OfficeId belongs to the GovernorateId using FirstOrDefaultAsync
                var office = await _unitOfWork.Repository<Office>()
                    .FirstOrDefaultAsync(o => o.Id == request.OfficeId && o.GovernorateId == request.GovernorateId);

                if (office == null)
                {
                    // If the office doesn't belong to the governorate, throw an exception
                    throw new Exception($"Office ID {request.OfficeId} does not belong to Governorate ID {request.GovernorateId}.");
                }

                // Step 2: Validate if the CompanyId exists and the LectureTypeId belongs to the specified Company
                var company = await _unitOfWork.Repository<Company>()
                    .FirstOrDefaultAsync(c => c.Id == request.CompanyId);

                if (company == null)
                {
                    throw new Exception($"Company ID {request.CompanyId} does not exist.");
                }

                var lectureType = await _unitOfWork.Repository<LectureType>()
                    .FirstOrDefaultAsync(lt => lt.Id == request.LectureTypeId && lt.CompanyId == request.CompanyId);

                if (lectureType == null)
                {
                    throw new Exception($"LectureType ID {request.LectureTypeId} does not belong to Company ID {request.CompanyId}.");
                }

                // Step 3: Map the command to the Lecture entity
                var lecture = _mapper.Map<Lecture>(request);

                // Optionally, set any additional properties or perform transformations before saving

                // Step 4: Add the lecture entity to the repository using AddAsync
                await _unitOfWork.Repository<Lecture>().AddAsync(lecture);

                // Step 5: Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to save the lecture to the database.");
                }

                // Step 6: Return the ID of the newly created lecture
                return lecture.Id;
            }
            catch (Exception ex)
            {
                // Handle the exception and throw a custom HandlerException
                throw new HandlerException("An error occurred while adding the lecture.", ex);
            }
        }
    }
}
