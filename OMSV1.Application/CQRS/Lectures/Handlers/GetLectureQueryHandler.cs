using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
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
                // Create the specification based on the query parameters, including CompanyId and LectureTypeId
                var spec = new FilterLecturesSpecification(
                    request.Title,
                    request.StartDate,
                    request.EndDate,
                    request.OfficeId,
                    request.GovernorateId,
                    request.ProfileId,
                    request.CompanyId, // Passing CompanyId to the specification
                    request.LectureTypeId // Passing LectureTypeId to the specification
                );

                // Get the queryable list of Lecture entities
                var queryableResult = _repository.ListAsQueryable(spec);

                // Map to LectureDto
                var mappedQuery = queryableResult.ProjectTo<LectureAllDto>(_mapper.ConfigurationProvider);

                // Create a paginated list of LectureDto
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
