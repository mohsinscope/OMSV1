using MediatR;
using Microsoft.AspNetCore.Http;

namespace OMSV1.Application.Commands.DamagedPassports
{
    public class AddDamagedPassportWithAttachmentCommand : IRequest<Guid>
    {
        // Damaged passport properties
        public required string PassportNumber { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; } = "";
        public Guid OfficeId { get; set; }
        public Guid GovernorateId { get; set; }
        public Guid DamagedTypeId { get; set; }
        public Guid ProfileId { get; set; }

        // Attachment property
        public required IFormFile File { get; set; }
    }
}
