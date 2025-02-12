using MediatR;

namespace OMSV1.Application.Commands.Offices
{
    public class UpdateOfficeCommand : IRequest<bool>
    {
        public Guid OfficeId { get; set; }
        public string Name { get; set; } = null!;
        public int Code { get; set; }
        public int ReceivingStaff { get; set; }
        public int AccountStaff { get; set; }
        public int PrintingStaff { get; set; }
        public int QualityStaff { get; set; }
        public int DeliveryStaff { get; set; }
        public decimal? Budget { get; set; } // Nullable Budget property
        public bool? IsEmbassy { get; set; }

    }
}
