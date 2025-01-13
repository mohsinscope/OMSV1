using MediatR;
using OMSV1.Application.Dtos.Lectures;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.CQRS.Lectures.Queries
{
    public class GetLectureQuery : IRequest<PagedList<LectureAllDto>>
    {
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? OfficeId { get; set; }
        public Guid? GovernorateId { get; set; }
        public Guid? ProfileId { get; set; }
        public Guid? CompanyId { get; set; }
        public List<Guid>? LectureTypeIds { get; set; } // Changed from Guid? to List<Guid>?
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public PaginationParams PaginationParams { get; set; }

        public GetLectureQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
