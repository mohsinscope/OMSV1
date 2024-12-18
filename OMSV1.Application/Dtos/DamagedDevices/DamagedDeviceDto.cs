using System;

namespace OMSV1.Application.Dtos.DamagedDevices;

public class DamagedDeviceDto
{
    public int Id { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime Date { get; set; }
    public int DeviceTypeId { get; set; }
    public string? DeviceTypeName { get; set; }
    public int GovernorateId { get; set; }
    public string? GovernorateName { get; set; }
    public int? officeId {get;set;}
    public string? OfficeName {get;set;}
    public int? ProfileId { get; set; }  
    public string? ProfileFullName{ get; set; }  
     public int? DamagedDeviceTypeId { get; set; } // New field for type ID
    public string? DamagedDeviceTypesName { get; set; } // New field for type name

}