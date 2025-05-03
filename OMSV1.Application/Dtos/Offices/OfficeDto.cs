namespace OMSV1.Application.Dtos.Offices
{
    public class OfficeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Code { get; set; }
        public int ReceivingStaff { get; set; }
        public int AccountStaff { get; set; }
        public int PrintingStaff { get; set; }
        public int QualityStaff { get; set; }
        public int DeliveryStaff { get; set; }
        public Guid GovernorateId { get; set; }
        public string? GovernorateName { get; set; } // New property
        public decimal? Budget { get; set; } // Nullable Budget property
        public bool? IsEmbassy { get; set; }
        public bool? IsTwoShifts { get; set; }



    }
}
