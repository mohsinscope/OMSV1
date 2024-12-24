namespace OMSV1.Application.Dtos.Attendances
{
    public class AttendanceAllDto
    {
        public int Id { get; set; }
        
        // Integer properties for staff
        public int ReceivingStaff { get; set; }
        public int AccountStaff { get; set; }
        public int PrintingStaff { get; set; }
        public int QualityStaff { get; set; }
        public int DeliveryStaff { get; set; }

        // DateTime for the date, should match the entity's Date field
        public DateTime Date { get; set; }


        // WorkingHours should be kept as an integer (since it's an enum in the entity)
        public int WorkingHours { get; set; }

        // Name properties for DTO output
        public string GovernorateName { get; set; }
        public string OfficeName { get; set; }
        public string ProfileFullName { get; set; }
    }
}
