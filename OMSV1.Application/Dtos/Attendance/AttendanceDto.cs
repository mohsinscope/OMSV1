namespace OMSV1.Application.Dtos.Attendances
{
    public class AttendanceDto
    {
        public Guid Id { get; set; }
        
        // Integer properties for staff
        public int ReceivingStaff { get; set; }
        public int AccountStaff { get; set; }
        public int PrintingStaff { get; set; }
        public int QualityStaff { get; set; }
        public int DeliveryStaff { get; set; }

        // DateTime for the date, should match the entity's Date field
        public DateTime Date { get; set; }

        // Note, should be string as it is in the entity
        public string? Note { get; set; }

        // WorkingHours should be kept as an integer (since it's an enum in the entity)
        public int WorkingHours { get; set; }

        // Foreign key relationships
        public Guid GovernorateId { get; set; }

        // Name properties for DTO output
        public string GovernorateName { get; set; }

        public Guid OfficeId { get; set; }
        public string OfficeName { get; set; }

        public Guid ProfileId { get; set; }
        public string ProfileFullName { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
