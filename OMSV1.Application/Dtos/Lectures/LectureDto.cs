using System;

namespace OMSV1.Application.Dtos.Lectures
{
    public class LectureDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }

        public Guid OfficeId { get; set; }
        public string OfficeName { get; set; } // Assuming you want to include Office Name
        public Guid GovernorateId { get; set; }
        public string GovernorateName { get; set; } // Assuming you want to include Governorate Name
        public Guid ProfileId { get; set; }
        public string ProfileFullName { get; set; } // Assuming you want to include Profile Full Name
        public Guid CompanyId {get;set;}
        public string CompanyName { get; set; } // Added Company Name
        public Guid LectureTypeId {get;set;}
        public string LectureTypeName { get; set; } // Added Lecture Type Name

        public LectureDto(string title, DateTime date,string note, Guid officeId, string officeName, Guid governorateId, 
                          string governorateName, Guid profileId, string profileFullName,Guid companyId,string companyName,Guid lectureTypeId,string lectureTypeName)
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
            CompanyId=companyId;
            CompanyName=companyName;
            LectureTypeId=lectureTypeId;
            LectureTypeName=lectureTypeName;
        }
    }
}
