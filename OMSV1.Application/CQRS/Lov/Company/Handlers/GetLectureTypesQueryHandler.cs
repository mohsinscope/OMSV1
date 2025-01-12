using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.LectureTypes;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Companies;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.LectureTypes
{

    public class GetAllLectureTypesQueryHandler : IRequestHandler<GetAllLectureTypesQuery, PagedList<LectureTypeAllDto>>
    {
        private readonly IGenericRepository<LectureType> _repository;
        private readonly IMapper _mapper;

        public GetAllLectureTypesQueryHandler(IGenericRepository<LectureType> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

    public async Task<PagedList<LectureTypeAllDto>> Handle(GetAllLectureTypesQuery request, CancellationToken cancellationToken)
{
    try
    {
        // Retrieve the lecture types as IQueryable with included related data
        var lectureTypesQuery = _repository.GetAllAsQueryable()
            .Include(lt => lt.LectureLectureTypes) // Eagerly load the related LectureLectureTypes
            .ThenInclude(llt => llt.Lecture); // Load Lecture for nested data

        // Map the result to LectureTypeAllDto using AutoMapper's ProjectTo
        var mappedQuery = lectureTypesQuery.ProjectTo<LectureTypeAllDto>(_mapper.ConfigurationProvider);

        // Apply pagination using PagedList
        var pagedLectureTypes = await PagedList<LectureTypeAllDto>.CreateAsync(
            mappedQuery,
            request.PaginationParams.PageNumber,
            request.PaginationParams.PageSize
        );

        return pagedLectureTypes;
    }
    catch (Exception ex)
    {
        // Catch and throw a custom exception for better error reporting
        throw new HandlerException("An error occurred while retrieving lecture types.", ex);
    }
}

    }
}
