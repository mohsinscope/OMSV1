using System;

namespace OMSV1.Application.Dtos.DamagedDevices;

public class DamagedDeviceDto
{
    public Guid Id { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime Date { get; set; }
    public Guid DeviceTypeId { get; set; }
    public string? DeviceTypeName { get; set; }
    public string? Note { get; set; }

    public Guid GovernorateId { get; set; }
    public string? GovernorateName { get; set; }
    public Guid? officeId {get;set;}
    public string? OfficeName {get;set;}
    public Guid? ProfileId { get; set; }  
    public string? ProfileFullName{ get; set; }  
     public Guid? DamagedDeviceTypeId { get; set; } // New field for type ID
    public string? DamagedDeviceTypesName { get; set; } // New field for type name
    public DateTime Datecreated { get; set; }


}