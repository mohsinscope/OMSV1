namespace OMSV1.Application.Dtos
{
    public class DamagedPassportDto
    {
        public Guid Id { get; set; }
        public string PassportNumber { get; set; }
        public DateTime Date { get; set; }
        public Guid DamagedTypeId { get; set; }
        public string DamagedTypeName { get; set; }  // Assuming this is populated from the DamagedType entity
        public string? Note { get; set; }
        public Guid OfficeId { get; set; }
        public string OfficeName { get; set; }  // Assuming this is populated from the Office entity
        public Guid GovernorateId { get; set; }
        public string GovernorateName { get; set; }  // Assuming this is populated from the Governorate entity
        public Guid ProfileId { get; set; }
        public string ProfileFullName { get; set; }  // Assuming this is populated from the Profile entity

        public DamagedPassportDto(string passportNumber, DateTime date, Guid damagedTypeId, string damagedTypeName,string note,
                                  Guid officeId, string officeName, Guid governorateId, string governorateName,
                                  Guid profileId, string profileFullName)
        {
            PassportNumber = passportNumber;
            Date = date;
            DamagedTypeId = damagedTypeId;
            DamagedTypeName = damagedTypeName;
            Note=note;
            OfficeId = officeId;
            OfficeName = officeName;
            GovernorateId = governorateId;
            GovernorateName = governorateName;
            ProfileId = profileId;
            ProfileFullName = profileFullName;
        }
    }
}
