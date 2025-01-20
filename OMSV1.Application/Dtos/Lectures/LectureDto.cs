using System;
using System.Collections.Generic;

namespace OMSV1.Application.Dtos.Lectures
{
    public class LectureDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }

        public Guid OfficeId { get; set; }
        public required string OfficeName { get; set; }
        public Guid GovernorateId { get; set; }
        public required string GovernorateName { get; set; }
        public Guid ProfileId { get; set; }
        public required string ProfileFullName { get; set; }
        public Guid CompanyId { get; set; }
        public required string CompanyName { get; set; }
        public required List<string> LectureTypeNames { get; set; }  // Assuming multiple Lecture Types

        // Default constructor (parameterless)
        public LectureDto() { }

        // Constructor with parameters for manual initialization
        public LectureDto(string title, DateTime date, string note, Guid officeId, string officeName, 
                          Guid governorateId, string governorateName, Guid profileId, string profileFullName, 
                          Guid companyId, string companyName, List<string> lectureTypeNames)
        {
            Title = title;
            Date = date;
            Note = note;
            OfficeId = officeId;
            OfficeName = officeName;
            GovernorateId = governorateId;
            GovernorateName = governorateName;
            ProfileId = profileId;
            ProfileFullName = profileFullName;
            CompanyId = companyId;
            CompanyName = companyName;
            LectureTypeNames = lectureTypeNames ?? new List<string>(); // Default to an empty list if null
        }
    }
}
