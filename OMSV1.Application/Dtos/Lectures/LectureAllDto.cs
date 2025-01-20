using System;
using System.Collections.Generic;

namespace OMSV1.Application.Dtos.Lectures
{
    public class LectureAllDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public DateTime Date { get; set; }
        public required string OfficeName { get; set; } // Office Name
        public required string GovernorateName { get; set; } // Governorate Name
        public required string CompanyName { get; set; } // Company Name

        // Added LectureTypeNames to hold multiple lecture type names
        public List<string> LectureTypeNames { get; set; } = new List<string>();

        // Default constructor
        public LectureAllDto() { }

        // Constructor to initialize the DTO
        public LectureAllDto(string title, DateTime date, string officeName, string governorateName, string companyName, List<string> lectureTypeNames)
        {
            Title = title;
            Date = date;
            OfficeName = officeName;
            GovernorateName = governorateName;
            CompanyName = companyName;
            LectureTypeNames = lectureTypeNames ?? new List<string>(); // Initialize with empty list if null
        }
    }
}
