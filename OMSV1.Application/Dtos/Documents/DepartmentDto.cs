// Application/Dtos/Departments/DepartmentsDto.cs
namespace OMSV1.Application.Dtos.Departments
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid DirectorateId { get; set; }
        public string DirectorateName      { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; }
    }
}
