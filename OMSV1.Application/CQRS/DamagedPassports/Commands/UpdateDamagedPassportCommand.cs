using MediatR;
using System;

namespace OMSV1.Application.Commands.DamagedPassports
{
    public class UpdateDamagedPassportCommand : IRequest<bool>
    {
        public int Id { get; set; }  // Unique identifier for the damaged passport
        public string PassportNumber { get; set; }
        public DateTime Date { get; set; }
        public int OfficeId { get; set; }
        public int GovernorateId { get; set; }
        public int DamagedTypeId { get; set; }
        public int ProfileId { get; set; } 
    }
}
