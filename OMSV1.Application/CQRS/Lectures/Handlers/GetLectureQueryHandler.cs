using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.CQRS.Lectures.Queries;
using OMSV1.Application.Dtos.Lectures;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Lectures;

namespace OMSV1.Application.CQRS.Lectures.Handlers
{
    public class GetLectureQueryHandler : IRequestHandler<GetLectureQuery, PagedList<LectureAllDto>>
    {
        private readonly IGenericRepository<Lecture> _repository;
        private readonly IMapper _mapper;

        public GetLectureQueryHandler(IGenericRepository<Lecture> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<LectureAllDto>> Handle(GetLectureQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Create the specification based on the query parameters
                var spec = new FilterLecturesSpecification(
                    request.Title,
                    request.StartDate,
                    request.EndDate,
                    request.OfficeId,
                    request.GovernorateId,
                    request.ProfileId,
                    request.CompanyId,
                    request.LectureTypeIds // Pass the list of LectureTypeIds
                );

                // Get the queryable list of Lecture entities
                var queryableResult = _repository.ListAsQueryable(spec)
                    .Include(l => l.LectureLectureTypes) // Include LectureTypes navigation property
                    .ThenInclude(lt => lt.LectureType); // Include LectureType entity

                // Map to LectureAllDto with LectureTypeNames
                var mappedQuery = queryableResult.Select(l => new LectureAllDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Date = l.Date,
                    OfficeName = l.Office.Name,
                    GovernorateName = l.Governorate.Name,
                    CompanyName = l.Company.Name,
                    LectureTypeNames = l.LectureLectureTypes.Select(lt => lt.LectureType.Name).ToList()
                });

                // Create a paginated list of LectureAllDto
                return await PagedList<LectureAllDto>.CreateAsync(mappedQuery, request.PaginationParams.PageNumber, request.PaginationParams.PageSize);
            }
            catch (Exception ex)
            {
                // Handle the exception and throw a custom HandlerException
                throw new HandlerException("An error occurred while fetching lectures.", ex);
            }
        }

    }
}
