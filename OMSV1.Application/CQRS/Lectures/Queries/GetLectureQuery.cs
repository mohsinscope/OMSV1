


using MediatR;
using OMSV1.Application.Dtos.Lectures;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.CQRS.Lectures.Queries;
public class GetLectureQuery : IRequest <PagedList<LectureDto>>
{
    public string Title { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? OfficeId { get; set; }
    public int? GovernorateId { get; set; }
    public int? ProfileId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public PaginationParams PaginationParams { get;set; }

    public GetLectureQuery (PaginationParams paginationParams)
    {
        PaginationParams = paginationParams;
    }


}