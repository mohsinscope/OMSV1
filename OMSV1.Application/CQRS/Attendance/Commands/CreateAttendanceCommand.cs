using MediatR;

namespace OMSV1.Application.Commands.Attendances
{
    public class CreateAttendanceCommand : IRequest<Guid>
    {
        // Staff details (all integers representing IDs)
        public int ReceivingStaff { get; set; } // ID for receiving staff
        public int AccountStaff { get; set; }   // ID for account staff
        public int PrintingStaff { get; set; }  // ID for printing staff
        public int QualityStaff { get; set; }   // ID for quality staff
        public int DeliveryStaff { get; set; }  // ID for delivery staff

        // Date and note (the same as in the entity)
        public DateTime Date { get; set; } // Use DateTime, default can be set in handler or on the client side
        public string Note { get; set; } = ""; // Default value for Note if not provided

        // Foreign key relations to Governorate, Office, Profile
        public Guid GovernorateId { get; set; } // FK to Governorate
        public Guid OfficeId { get; set; }      // FK to Office
        public Guid ProfileId { get; set; }     // FK to Profile

        // Working hours (use an integer, matching how it's stored in the entity)
        public int WorkingHours { get; set; }  // Integer value for WorkingHours (enum mapped as int)
    }
}
