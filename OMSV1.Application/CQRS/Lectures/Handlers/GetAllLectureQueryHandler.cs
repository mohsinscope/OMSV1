using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Lectures;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Lectures;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;

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
                // Retrieve the lectures as IQueryable
                var lecturesQuery = _repository.GetAllAsQueryable();
                // Apply ordering here - replace 'Date' with the field you want to order by
                lecturesQuery = lecturesQuery.OrderByDescending(dp => dp.Date);  // Example: Order by Date in descending order

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
                // Handle the exception and throw a custom HandlerException
                throw new HandlerException("An error occurred while fetching the lectures.", ex);
            }
        }
    }
}
