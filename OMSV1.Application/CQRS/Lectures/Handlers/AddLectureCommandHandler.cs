using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Lectures;
using OMSV1.Domain.Entities.Lectures;
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
            // Create a new Lecture instance using the data from the command
            var lecture = _mapper.Map<Lecture>(request);

            // Optionally, set any additional properties or perform transformations before saving

            await _unitOfWork.Repository<Lecture>().AddAsync(lecture);

            // Save the changes to the database
            if (!await _unitOfWork.SaveAsync(cancellationToken))
            {
                throw new Exception("Failed to save the lecture to the database.");
            }

            // Return the ID of the newly created Lecture
            return lecture.Id;
        }
    }
}
