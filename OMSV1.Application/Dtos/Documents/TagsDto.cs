// MinistryDto.cs
namespace OMSV1.Application.Dtos.Documents
{
    public class TagsDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
