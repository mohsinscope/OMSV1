using MediatR;
using OMSV1.Application.Dtos.LectureTypes;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Companies;
public class GetAllLectureTypesQuery : IRequest<PagedList<LectureTypeAllDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllLectureTypesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }