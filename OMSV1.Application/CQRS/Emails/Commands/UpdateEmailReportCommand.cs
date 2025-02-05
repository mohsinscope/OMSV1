using MediatR;
using System;
using System.Collections.Generic;

namespace OMSV1.Application.Commands.Reports
{
    public class UpdateEmailReportCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<Guid> ReportTypeIds { get; set; } = new List<Guid>();

        public UpdateEmailReportCommand(Guid id, string fullName, string email, List<Guid> reportTypeIds)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            ReportTypeIds = reportTypeIds;
        }
    }
}
