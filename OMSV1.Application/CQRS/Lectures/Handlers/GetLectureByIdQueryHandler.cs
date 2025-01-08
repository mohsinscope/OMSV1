using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Lectures;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Lectures;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.SeedWork;
using Microsoft.Extensions.Logging; // Ensure you are using a logging library

namespace OMSV1.Application.Handlers.Lectures
{
    public class GetLectureByIdQueryHandler : IRequestHandler<GetLectureByIdQuery, LectureDto?>
    {
        private readonly IGenericRepository<Lecture> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetLectureByIdQueryHandler> _logger; // Add a logger

        public GetLectureByIdQueryHandler(IGenericRepository<Lecture> repository, IMapper mapper, ILogger<GetLectureByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger; // Initialize logger
        }

public async Task<LectureDto?> Handle(GetLectureByIdQuery request, CancellationToken cancellationToken)
{
    try
    {
        // Retrieve the lecture and include the related LectureTypes
        var lectureQuery = _repository.GetAllAsQueryable()
            .Where(l => l.Id == request.Id)
            .Include(l => l.LectureLectureTypes) // Include the LectureLectureTypes
            .ThenInclude(llt => llt.LectureType); // Include the related LectureType

        // Use ProjectTo before calling FirstOrDefaultAsync to properly project the query to the DTO
        var lectureDto = await lectureQuery
            .ProjectTo<LectureDto>(_mapper.ConfigurationProvider) // Project to LectureDto
            .FirstOrDefaultAsync(cancellationToken); // Fetch the first result or null

        // Check if the LectureDto has no associated LectureTypes
        if (lectureDto?.LectureTypeNames == null)
        {
            // Log or handle case where no LectureType is found
            _logger.LogWarning($"Lecture with ID: {request.Id} has no associated LectureTypes.");
        }

        return lectureDto;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error occurred while fetching the lecture by ID.");
        throw new HandlerException("An error occurred while fetching the lecture by ID.", ex);
    }
}

    }
}
