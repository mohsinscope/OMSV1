using MediatR;
using OMSV1.Application.DTOs.Reports;
using System;

namespace OMSV1.Application.Queries.Reports
{
    public class GetEmailReportByIdQuery : IRequest<EmailReportDto>
    {
        public Guid Id { get; }

        public GetEmailReportByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
