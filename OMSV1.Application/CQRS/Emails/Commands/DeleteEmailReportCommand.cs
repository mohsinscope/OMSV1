using MediatR;
using System;

namespace OMSV1.Application.Commands.Reports
{
    public class DeleteEmailReportCommand : IRequest<bool>
    {
        public Guid Id { get; }

        public DeleteEmailReportCommand(Guid id)
        {
            Id = id;
        }
    }
}
