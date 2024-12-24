using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Lectures;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Application.Handlers.Lectures
{
    public class AddLectureCommandHandler : IRequestHandler<AddLectureCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public AddLectureCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        public async Task<int> Handle(AddLectureCommand request, CancellationToken cancellationToken)
        {
            // Step 1: Validate if the OfficeId belongs to the GovernorateId using FirstOrDefaultAsync
            var office = await _unitOfWork.Repository<Office>()
                .FirstOrDefaultAsync(o => o.Id == request.OfficeId && o.GovernorateId == request.GovernorateId);

            if (office == null)
            {
                // If the office doesn't belong to the governorate, throw an exception
                throw new Exception($"Office ID {request.OfficeId} does not belong to Governorate ID {request.GovernorateId}.");
            }

            // Step 2: Map the command to the Lecture entity
            var lecture = _mapper.Map<Lecture>(request);

            // Optionally, set any additional properties or perform transformations before saving

            // Step 3: Add the lecture entity to the repository using AddAsync
            await _unitOfWork.Repository<Lecture>().AddAsync(lecture);

            // Step 4: Save changes to the database
            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                throw new Exception("Failed to save the lecture to the database.");
            }

            // Step 5: Return the ID of the newly created lecture
            return lecture.Id;
        }
    }
}
