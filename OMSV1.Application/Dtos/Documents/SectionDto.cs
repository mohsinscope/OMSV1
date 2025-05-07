// Application/Dtos/Sections/SectionsDto.cs
namespace OMSV1.Application.Dtos.Sections
{
    public class SectionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid DepartmentId { get; set; }
        public string DepartmentName      { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; }
    }
}
