using System;

namespace OMSV1.Application.Dtos.DamagedDevices;

public class DamagedDeviceAllDto
{
    public int Id { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime Date { get; set; }
    public string? DeviceTypeName { get; set; }
    public string? OfficeName {get;set;}
    public string? GovernorateName { get; set; }
    public string? DamagedDeviceTypesName { get; set; } // New field for type name

}