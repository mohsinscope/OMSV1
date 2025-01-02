using MediatR;

namespace OMSV1.Application.Commands.DamagedPassports
{
    public class AddDamagedPassportCommand : IRequest<Guid>
    {
        public string PassportNumber { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; } = ""; // Default value for Note if not provided
        public Guid OfficeId { get; set; }
        public Guid GovernorateId { get; set; }
        public Guid DamagedTypeId { get; set; }
        public Guid ProfileId { get; set; } 

    }
}
