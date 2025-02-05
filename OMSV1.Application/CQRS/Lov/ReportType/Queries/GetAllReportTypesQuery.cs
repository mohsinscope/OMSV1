using MediatR;
using OMSV1.Application.DTOs.Reports;
using System.Collections.Generic;

namespace OMSV1.Application.Queries.Reports
{
    public class GetAllReportTypesQuery : IRequest<List<ReportTypeDto>>
    {
    }
}
