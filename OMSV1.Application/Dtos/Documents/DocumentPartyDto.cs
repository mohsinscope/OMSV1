using OMSV1.Domain.Enums;

namespace OMSV1.Application.Dtos.Documents
{
    public class DocumentPartyDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public PartyType PartyType { get; set; }
        public bool IsOfficial { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
