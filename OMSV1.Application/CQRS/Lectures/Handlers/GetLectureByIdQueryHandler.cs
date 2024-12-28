using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Lectures;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Lectures;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Lectures
{
    public class GetLectureByIdQueryHandler : IRequestHandler<GetLectureByIdQuery, LectureDto?>
    {
        private readonly IGenericRepository<Lecture> _repository;
        private readonly IMapper _mapper;

        public GetLectureByIdQueryHandler(IGenericRepository<Lecture> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LectureDto?> Handle(GetLectureByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the specific lecture as an IQueryable
                var lectureQuery = _repository.GetAllAsQueryable()
                    .Where(l => l.Id == request.Id); // Filter by the given ID

                // Map to LectureDto using AutoMapper's ProjectTo
                var lectureDto = await lectureQuery
                    .ProjectTo<LectureDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken); // Fetch the first result or null

                return lectureDto; // Return the mapped DTO
            }
            catch (Exception ex)
            {
                // Handle the exception and throw a custom HandlerException
                throw new HandlerException("An error occurred while fetching the lecture by ID.", ex);
            }
        }
    }
}
