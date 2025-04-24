using OMSV1.Application.Dtos.Documents;

namespace OMSV1.Application.Dtos.Projects
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime DateCreated { get; set; }

        // Related documents
        //public ICollection<DocumentDto> Documents { get; set; } = new List<DocumentDto>();

        // Related parties
       // public ICollection<DocumentPartyDto> Parties { get; set; } = new List<DocumentPartyDto>();
    }
}