using System;

namespace OMSV1.Application.Dtos.Lectures
{
    public class LectureAllDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string OfficeName { get; set; } // Assuming you want to include Office Name
        public string GovernorateName { get; set; } // Assuming you want to include Governorate Name
        public string CompanyName { get; set; } // Added Company Name
        public string LectureTypeName { get; set; } // Added Lecture Type Name

        // Constructor to initialize the DTO
        public LectureAllDto(string title, DateTime date, string officeName, string governorateName, string companyName, string lectureTypeName)
        {
            Title = title;
            Date = date;
            OfficeName = officeName;
            GovernorateName = governorateName;
            CompanyName = companyName;
            LectureTypeName = lectureTypeName;
        }
    }
}
