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
        public LectureAllDto(string title, DateTime date, string officeName,string governorateName)
        {
            Title = title;
            Date = date;
            OfficeName = officeName;
            GovernorateName = governorateName;
        }
    }
}
