namespace OMSV1.Application.Dtos.Documents
{
    public class DocumentPartyDto
    {
        // The name of the document party
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
