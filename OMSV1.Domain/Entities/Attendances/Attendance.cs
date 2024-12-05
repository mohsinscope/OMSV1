using System;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Enums;
namespace OMSV1.Domain.Entities.Attendances;

public class Attendance(string name,
                    int receivingStaff,
                    int accountStaff,
                    int printingStaff,
                    int qualityStaff,
                    int deliveryStaff,
                    DateTime date,
                    string note,
                    WorkingHours workingHours,
                    int governorateId,
                    int officeId
                   ) : Entity
{
    public string Name { get; private set; } = name;
    public int ReceivingStaff { get; private set; } = receivingStaff;
    public int AccountStaff { get; private set; } = accountStaff;
    public int PrintingStaff { get; private set; } = printingStaff;
    public int QualityStaff { get; private set; } = qualityStaff;
    public int DeliveryStaff { get; private set; } = deliveryStaff;
    public DateTime Date { get; private set; } = date;
    public string Note { get; private set; } = note;
    public WorkingHours WorkingHours { get; private set; } = workingHours;
    public int OfficeId { get; private set; } = officeId;
    public int GovernorateId { get; private set; } = governorateId;
    public Governorate? Governorate { get; private set; }
    public Office? Office { get; private set; }

}
