// DocumentCCDto.cs
namespace OMSV1.Application.Dtos.Documents
{
    public class DocumentCCDto
    {
        public Guid Id { get; set; }
        public required string RecipientName { get; set; }
    }
}
