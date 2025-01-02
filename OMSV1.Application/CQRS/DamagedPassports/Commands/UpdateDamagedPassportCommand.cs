using MediatR;

namespace OMSV1.Application.Commands.DamagedPassports
{
    public class UpdateDamagedPassportCommand : IRequest<bool>
    {
        public Guid Id { get; set; }  // Unique identifier for the damaged passport
        public string PassportNumber { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }

        public Guid OfficeId { get; set; }
        public Guid GovernorateId { get; set; }
        public Guid DamagedTypeId { get; set; }
        public Guid ProfileId { get; set; } 
    }
}
