using MediatR;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Commands.Attendances
{
    public class UpdateAttendanceCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public int ReceivingStaff { get; set; }
        public int AccountStaff { get; set; }
        public int PrintingStaff { get; set; }
        public int QualityStaff { get; set; }
        public int DeliveryStaff { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }
        
        // Change to integer, as it is stored as integer in the entity
        public int WorkingHours { get; set; } // Using integer instead of enum
        
        public int GovernorateId { get; set; }
        public int OfficeId { get; set; }
        public int ProfileId { get; set; }
    }
}
