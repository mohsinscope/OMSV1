namespace OMSV1.Application.Dtos.Attendances
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public int ReceivingStaff { get; set; } =1;
        public int AccountStaff { get; set; }=1;
        public int PrintingStaff { get; set; }
        public int QualityStaff { get; set; }
        public int DeliveryStaff { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public string WorkingHours { get; set; }
        public int GovernorateId { get; set; }
        public string GovernorateName { get; set; }
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        public int ProfileId { get; set; }
        public string ProfileFullName { get; set; }
    }
}
