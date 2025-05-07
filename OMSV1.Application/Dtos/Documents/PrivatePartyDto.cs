// Application/Dtos/Documents/PrivatePartyDto.cs
namespace OMSV1.Application.Dtos.Documents
{
    public class PrivatePartyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
       // public int DocumentCount { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
