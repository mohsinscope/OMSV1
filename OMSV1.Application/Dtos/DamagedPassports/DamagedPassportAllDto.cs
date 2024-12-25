namespace OMSV1.Application.Dtos
{
    public class DamagedPassportAllDto
    {
        public int Id { get; set; }
        public string PassportNumber { get; set; }
        public DateTime Date { get; set; }
        public string DamagedTypeName { get; set; }  // Assuming this is populated from the DamagedType entity
        public string OfficeName { get; set; }  // Assuming this is populated from the Office entity
        public string GovernorateName { get; set; }  // Assuming this is populated from the Governorate entity
        public DamagedPassportAllDto(string passportNumber, DateTime date, string damagedTypeName, string officeName,  string governorateName)
        {
            PassportNumber = passportNumber;
            Date = date;
            DamagedTypeName = damagedTypeName;
            OfficeName = officeName;
            GovernorateName = governorateName;
        }
    }
}
