using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.Entities.Reports;
using OMSV1.Domain.Interfaces;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Infrastructure.Persistence;

namespace OMSV1.Infrastructure.Repositories
{
    public class EmailReportRepository : IEmailReportRepository
    {
        private readonly AppDbContext _context;

        public EmailReportRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves the email addresses of users subscribed to a specific report type.
        /// </summary>
        /// <param name="reportTypeName">The name of the report type (e.g., "Incident Report").</param>
        /// <returns>A collection of email addresses.</returns>
        public async Task<IEnumerable<string>> GetEmailsByReportTypeAsync(string reportTypeName)
        {
            var emails = await _context.EmailReports
                .Where(er => er.ReportTypes.Any(rt => rt.Name == reportTypeName))
                .Select(er => er.Email)
                .Distinct() // Remove duplicates if any
                .ToListAsync();

            return emails;
        }
    }
}
