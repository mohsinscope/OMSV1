using MediatR;
namespace OMSV1.Application.Commands.Reports
{
    public class AddEmailReportCommand : IRequest<Guid> // Returns the ID of the newly created EmailReport
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<Guid> ReportTypeIds { get; set; } // List of ReportType IDs

        public AddEmailReportCommand(string fullName, string email, List<Guid> reportTypeIds)
        {
            FullName = fullName;
            Email = email;
            ReportTypeIds = reportTypeIds ?? new List<Guid>();
        }
    }
}
