namespace OMSV1.Application.Dtos.Attendances
{
    public class AttendanceStatisticsInOfficeDto
    {
        public int TotalStaffInOffice { get; set; }
        public int AvailableStaffInOffice { get; set; }
        public int ReceivingStaffTotal { get; set; }
        public int ReceivingStaffAvailable { get; set; }
        public int AccountStaffTotal { get; set; }
        public int AccountStaffAvailable { get; set; }
        public int PrintingStaffTotal { get; set; }
        public int PrintingStaffAvailable { get; set; }
        public int QualityStaffTotal { get; set; }
        public int QualityStaffAvailable { get; set; }
        public int DeliveryStaffTotal { get; set; }
        public int DeliveryStaffAvailable { get; set; }

    }
}
