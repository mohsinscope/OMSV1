using System;

namespace OMSV1.Application.Dtos.DamagedDevices;

public class DamagedDeviceDto
{
    public int Id { get; set; }
    public string SerialNumber { get; set; }
    public DateTime Date { get; set; }
    public int DeviceTypeId { get; set; }
    public string DeviceTypeName { get; set; }
    public int GovernorateId { get; set; }
    public string GovernorateName { get; set; }
    public string officeId {get;set;}
    public string OfficeName {get;set;}
}