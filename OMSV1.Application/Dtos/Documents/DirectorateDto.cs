// Application/Dtos/Directorates/DirectorateDto.cs
namespace OMSV1.Application.Dtos.Directorates
{
    public class DirectorateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid GeneralDirectorateId { get; set; }
        public string GeneralDirectorateName      { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; }
    }
}
