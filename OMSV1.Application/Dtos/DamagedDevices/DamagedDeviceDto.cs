namespace OMSV1.Application.Dtos.DamagedDevices;

public class DamagedDeviceDto
{
    public int Id { get; set; }
    public string SerialNumber { get; set; }
    public int DamagedTypeId { get; set; }
    public string DamagedTypeName { get; set; }
    public int OfficeId { get; set; }
    public string OfficeName { get; set; }
    public DateTime Date { get; set; }
    public int ProfileId { get; set; }
}
