using MediatR;
using OMSV1.Application.DTOs.Reports;
using OMSV1.Application.Helpers;
using System.Collections.Generic;

namespace OMSV1.Application.Queries.Reports
{
    public class GetAllEmailReportsQuery : IRequest<PagedList<EmailReportDto>>
    {
        public PaginationParams PaginationParams { get; set; }

        public GetAllEmailReportsQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
