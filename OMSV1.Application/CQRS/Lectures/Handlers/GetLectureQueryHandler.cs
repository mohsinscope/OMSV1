using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.CQRS.Lectures.Queries;
using OMSV1.Application.Dtos;
using OMSV1.Application.Dtos.Lectures;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Lectures;
using System;

namespace OMSV1.Application.CQRS.Lectures.Handlers
{
    public class GetLectureQueryHandler : IRequestHandler<GetLectureQuery, PagedList<LectureDto>>
    {
        private readonly IGenericRepository<Lecture> _repository;
        private readonly IMapper _mapper;

        public GetLectureQueryHandler(IGenericRepository<Lecture> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<LectureDto>> Handle(GetLectureQuery request, CancellationToken cancellationToken)
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
                    request.ProfileId);

                // Get the queryable list of Lecture entities
                var queryableResult = _repository.ListAsQueryable(spec);

                // Map to LectureDto
                var mappedQuery = queryableResult.ProjectTo<LectureDto>(_mapper.ConfigurationProvider);

                // Create a paginated list of LectureDto
                return await PagedList<LectureDto>.CreateAsync(mappedQuery, request.PaginationParams.PageNumber, request.PaginationParams.PageSize);
            }
            catch (Exception ex)
            {
                // Handle the exception and throw a custom HandlerException
                throw new HandlerException("An error occurred while fetching lectures.", ex);
            }
        }
    }
}
