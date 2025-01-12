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
    public class GetAllLecturesQueryHandler : IRequestHandler<GetAllLecturesQuery, PagedList<LectureAllDto>>
    {
        private readonly IGenericRepository<Lecture> _repository;
        private readonly IMapper _mapper;

        public GetAllLecturesQueryHandler(IGenericRepository<Lecture> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<LectureAllDto>> Handle(GetAllLecturesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the lectures as IQueryable and include LectureLectureTypes and LectureTypes
                var lecturesQuery = _repository.GetAllAsQueryable()
                    .Include(l => l.LectureLectureTypes) // Include the many-to-many relationship
                    .ThenInclude(llt => llt.LectureType); // Include the LectureType details


                // Map to LectureAllDto using AutoMapper's ProjectTo
                var mappedQuery = lecturesQuery.ProjectTo<LectureAllDto>(_mapper.ConfigurationProvider);

                // Apply pagination using PagedList
                var pagedLectures = await PagedList<LectureAllDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return pagedLectures;  // Return the paginated list
            }
          catch (Exception ex)
{
    // Log or examine the inner exception to get more details
    throw new HandlerException($"An error occurred while fetching the lectures. {ex.Message}", ex);
}

        }
    }
}
