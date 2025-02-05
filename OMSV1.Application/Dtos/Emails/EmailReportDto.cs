using System;
using System.Collections.Generic;

namespace OMSV1.Application.DTOs.Reports
{
    public class EmailReportDto
    {
        public Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        
        // If you wish to return detailed ReportType info, use ReportTypeDto
        public List<ReportTypeDto> ReportTypes { get; set; } = new List<ReportTypeDto>();
    }
}
