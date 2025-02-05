using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace OMSV1.Domain.Entities.Reports
{
    public class EmailReport : Entity
    {
        public string FullName { get; private set; }
        public string Email { get; private set; }

        // Many-to-Many Relationship with ReportType
        private readonly List<ReportType> _reportTypes = new();
        public IReadOnlyCollection<ReportType> ReportTypes => _reportTypes.AsReadOnly();

        // Constructor for full initialization
        public EmailReport(string fullName, string email)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Full Name cannot be empty", nameof(fullName));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty", nameof(email));

            FullName = fullName;
            Email = email;
        }

        // Method to add a report type
        public void AddReportType(ReportType reportType)
        {
            if (reportType == null) throw new ArgumentNullException(nameof(reportType));

            _reportTypes.Add(reportType);
        }
        //Update Email Report
        public void UpdateEmailReport(string fullName, string email)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full Name cannot be empty", nameof(fullName));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));
            
            FullName = fullName;
            Email = email;
        }


        // Method to remove a report type
        public void RemoveReportType(ReportType reportType)
        {
            if (reportType == null) throw new ArgumentNullException(nameof(reportType));

            _reportTypes.Remove(reportType);
        }
    }
}
