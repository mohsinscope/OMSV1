using MediatR;
using OMSV1.Application.Dtos.Lectures;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Lectures
{
    public class GetAllLecturesQuery : IRequest<PagedList<LectureAllDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllLecturesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
