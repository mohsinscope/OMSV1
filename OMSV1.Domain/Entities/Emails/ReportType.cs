using OMSV1.Domain.SeedWork;
using System;

namespace OMSV1.Domain.Entities.Reports
{
    public class ReportType : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        // Constructor
        public ReportType(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty", nameof(name));
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description cannot be empty", nameof(description));

            Name = name;
            Description = description;
        }

        // Method to update details
        public void UpdateReportType(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty", nameof(name));
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description cannot be empty", nameof(description));

            Name = name;
            Description = description;
        }
    }
}
