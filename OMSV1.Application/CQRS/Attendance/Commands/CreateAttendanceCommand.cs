using MediatR;

namespace OMSV1.Application.Commands.Attendances
{
 public class CreateAttendanceCommand : IRequest<int>
{
    public int ReceivingStaff { get; set; }  // Default value
    public int AccountStaff { get; set; } 
    public int PrintingStaff { get; set; }
    public int QualityStaff { get; set; } 
    public int DeliveryStaff { get; set; } 
    public DateTime Date { get; set; } // Default to current time
    public string? Note { get; set; } 
    public int GovernorateId { get; set; }// Assume a default governorate
    public int OfficeId { get; set; }// Assume a default office
    public int ProfileId { get; set; } // Assume a default profile
    public int WorkingHours { get; set; } // Default working hours
}
}
