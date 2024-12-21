using System;

namespace OMSV1.Application.Dtos.Lectures
{
    public class LectureDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }

        public int OfficeId { get; set; }
        public string OfficeName { get; set; } // Assuming you want to include Office Name
        public int GovernorateId { get; set; }
        public string GovernorateName { get; set; } // Assuming you want to include Governorate Name
        public int ProfileId { get; set; }
        public string ProfileFullName { get; set; } // Assuming you want to include Profile Full Name

        public LectureDto(string title, DateTime date,string note, int officeId, string officeName, int governorateId, 
                          string governorateName, int profileId, string profileFullName)
        {
            Title = title;
            Date = date;
            Note= note;
            OfficeId = officeId;
            OfficeName = officeName;
            GovernorateId = governorateId;
            GovernorateName = governorateName;
            ProfileId = profileId;
            ProfileFullName = profileFullName;
        }
    }
}
