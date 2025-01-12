using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Lectures;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.Entities.Companies;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using System.Linq;

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
                // Step 1: Validate if the OfficeId belongs to the GovernorateId
                var office = await _unitOfWork.Repository<Office>()
                    .FirstOrDefaultAsync(o => o.Id == request.OfficeId && o.GovernorateId == request.GovernorateId);

                if (office == null)
                {
                    throw new Exception($"Office ID {request.OfficeId} does not belong to Governorate ID {request.GovernorateId}.");
                }

                // Step 2: Validate if the CompanyId exists
                var company = await _unitOfWork.Repository<Company>()
                    .FirstOrDefaultAsync(c => c.Id == request.CompanyId);

                if (company == null)
                {
                    throw new Exception($"Company ID {request.CompanyId} does not exist.");
                }

                // Step 3: Validate all LectureTypes belong to the specified Company
                var validLectureTypes = new List<LectureType>();
                foreach (var lectureTypeId in request.LectureTypeIds)
                {
                    var lectureType = await _unitOfWork.Repository<LectureType>()
                        .FirstOrDefaultAsync(lt => lt.Id == lectureTypeId && lt.CompanyId == request.CompanyId);
                    
                    if (lectureType == null)
                    {
                        throw new Exception($"LectureType ID {lectureTypeId} does not exist or does not belong to Company ID {request.CompanyId}.");
                    }
                    
                    validLectureTypes.Add(lectureType);
                }

                // Step 4: Create the lecture without LectureTypes first
                var lecture = new Lecture(
                    request.Title,
                    request.Date,
                    request.Note,
                    request.OfficeId,
                    request.GovernorateId,
                    request.ProfileId,
                    request.CompanyId
                );

                // Step 5: Add the lecture entity to the repository
                await _unitOfWork.Repository<Lecture>().AddAsync(lecture);

                // Step 6: Add LectureTypes to the lecture
                foreach (var lectureTypeId in request.LectureTypeIds)
                {
                    lecture.AddLectureType(lectureTypeId);
                }

                // Step 7: Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to save the lecture to the database.");
                }

                // Step 8: Return the ID of the newly created lecture
                return lecture.Id;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while adding the lecture.", ex);
            }
        }
    }
}