


using MediatR;
using OMSV1.Application.Dtos.Attendances;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.CQRS.Attendances;

public class GetAttendanceQuery : IRequest<PagedList<AttendanceDto>>
{
    // Staff details (all integers representing IDs)
       // public int ReceivingStaff { get; set; } // ID for receiving staff
       // public int AccountStaff { get; set; }   // ID for account staff
       // public int PrintingStaff { get; set; }  // ID for printing staff
       // public int QualityStaff { get; set; }   // ID for quality staff
        //public int DeliveryStaff { get; set; }  // ID for delivery staff
        public int WorkingHours { get; set; }  // Integer value for WorkingHours (enum mapped as int)

    // Date
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
        //Others
    public int? GovernorateId { get; set; }
    public int? OfficeId { get; set; }
    public int? ProfileId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public PaginationParams PaginationParams { get;set; }

    public GetAttendanceQuery(PaginationParams paginationParams)
    {
        PaginationParams = paginationParams;
    }


}
