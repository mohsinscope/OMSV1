using MediatR;

namespace OMSV1.Application.Commands.DamagedPassports
{
    public class AddDamagedPassportCommand : IRequest<int>
    {
        public string PassportNumber { get; set; }
        public DateTime Date { get; set; }
        public int OfficeId { get; set; }
        public int GovernorateId { get; set; }
        public int DamagedTypeId { get; set; }
        public int ProfileId { get; set; } 

    }
}
