namespace OMSV1.Application.Dtos.Offices
{
    public class CreateOfficeDto
    {
        public string Name { get; set; } = null!;
        public int Code { get; set; }
        public int ReceivingStaff { get; set; }
        public int AccountStaff { get; set; }
        public int PrintingStaff { get; set; }
        public int QualityStaff { get; set; }
        public int DeliveryStaff { get; set; }
        public int GovernorateId { get; set; }
    }
}
