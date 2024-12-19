using System;
using System.Collections.Generic;
using OMSV1.Domain.Entities.Attachments;

namespace OMSV1.Application.Dtos
{
    public class DamagedPassportDto
    {
         public int Id { get; set; }
        public string PassportNumber { get; set; }
        public DateTime Date { get; set; }
        public int DamagedTypeId { get; set; }
        public string DamagedTypeName { get; set; }  // Assuming this is populated from the DamagedType entity
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }  // Assuming this is populated from the Office entity
        public int GovernorateId { get; set; }
        public string GovernorateName { get; set; }  // Assuming this is populated from the Governorate entity
        public int ProfileId { get; set; }
        public string ProfileFullName { get; set; }  // Assuming this is populated from the Profile entity
        public IReadOnlyCollection<DamagedPassportAttachmentDto> Attachments { get; set; }

        public DamagedPassportDto(string passportNumber, DateTime date, int damagedTypeId, string damagedTypeName,
                                  int officeId, string officeName, int governorateId, string governorateName,
                                  int profileId, string profileFullName, IReadOnlyCollection<DamagedPassportAttachmentDto> attachments)
        {
            PassportNumber = passportNumber;
            Date = date;
            DamagedTypeId = damagedTypeId;
            DamagedTypeName = damagedTypeName;
            OfficeId = officeId;
            OfficeName = officeName;
            GovernorateId = governorateId;
            GovernorateName = governorateName;
            ProfileId = profileId;
            ProfileFullName = profileFullName;
            Attachments = attachments;
        }
    }

    // Renamed DTO for Attachment to avoid conflict
    public class DamagedPassportAttachmentDto
    {
        public string FileName { get; set; }
        public string FilePath  { get; set; }  // Assuming URL is needed
    }
}
