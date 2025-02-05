using MediatR;
using System;

namespace OMSV1.Application.Commands.Reports
{
    public class UpdateReportTypeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public UpdateReportTypeCommand(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
