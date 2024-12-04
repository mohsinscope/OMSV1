using System;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Offices;

public class Office(string name,
                    int code,
                    int receivingStaff,
                    int accountStaff,
                    int printingStaff,
                    int qualityStaff,
                    int deliveryStaff,
                    int GovernorateId) : Entity
{
    public string Name { get; private set; } = name;
    public int Code { get; private set; } = code;
    public int ReceivingStaff { get; private set; } = receivingStaff;
    public int AccountStaff { get; private set; } = accountStaff;
    public int PrintingStaff { get; private set; } = printingStaff;
    public int QualityStaff { get; private set; } = qualityStaff;
    public int DeliveryStaff { get; private set; } = deliveryStaff;
    public int GovernorateId { get; private set; } = GovernorateId;
    public Governorate? Governorate { get; private set; }

}
