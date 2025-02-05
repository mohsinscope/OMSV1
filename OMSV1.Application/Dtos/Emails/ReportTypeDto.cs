namespace OMSV1.Application.DTOs.Reports
{
    public class ReportTypeDto
    {
        public Guid Id { get; set; }          // Unique identifier from Entity base class
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
