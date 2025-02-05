using System.Collections.Generic;
using System.Threading.Tasks;

namespace OMSV1.Infrastructure.Interfaces
{
    /// <summary>
    /// Repository for querying email reports and associated recipient email addresses based on report types.
    /// </summary>
    public interface IEmailReportRepository
    {
        /// <summary>
        /// Retrieves the email addresses of users subscribed to a specific report type.
        /// </summary>
        /// <param name="reportTypeName">The name of the report type (e.g., "Incident Report").</param>
        /// <returns>A collection of email addresses.</returns>
        Task<IEnumerable<string>> GetEmailsByReportTypeAsync(string reportTypeName);
    }
}
