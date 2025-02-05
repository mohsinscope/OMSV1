using MediatR;

namespace OMSV1.Application.Commands.Reports
{
    public class AddReportTypeCommand : IRequest<Guid> // Returns the ID of the newly created ReportType
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public AddReportTypeCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
