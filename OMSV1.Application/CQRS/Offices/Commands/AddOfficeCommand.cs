using MediatR;

namespace OMSV1.Application.Commands.Offices
{
    public class AddOfficeCommand : IRequest<Guid>
    {
        public required string Name { get; set; }
        public int Code { get; set; }
        public int ReceivingStaff { get; set; }
        public int AccountStaff { get; set; }
        public int PrintingStaff { get; set; }
        public int QualityStaff { get; set; }
        public int DeliveryStaff { get; set; }
        public Guid GovernorateId { get; set; }
        public decimal? Budget { get; set; } // Ensure this is included
    }

}
