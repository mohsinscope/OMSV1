using MediatR;

namespace OMSV1.Application.Commands.Office
{
    public class AddOfficeCommand : IRequest<int>
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public int ReceivingStaff { get; set; }
        public int AccountStaff { get; set; }
        public int PrintingStaff { get; set; }
        public int QualityStaff { get; set; }
        public int DeliveryStaff { get; set; }
        public int GovernorateId { get; set; }
    }

}
