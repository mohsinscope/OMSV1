using System;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Offices;

public class Office(string name,
                    string code,
                    string receivingStaff,
                    string accountStaff,
                    string printingStaff,
                    string qualityStaff,
                    string deliveryStaff,
                    int GovernorateId) : Entity
{
    public string Name { get; private set; } = name;
    public string Code { get; private set; } = code;
    public string ReceivingStaff { get; private set; } = receivingStaff;
    public string AccountStaff { get; private set; } = accountStaff;
    public string PrintingStaff { get; private set; } = printingStaff;
    public string QualityStaff { get; private set; } = qualityStaff;
    public string DeliveryStaff { get; private set; } = deliveryStaff;
    public int GovernorateId { get; private set; } = GovernorateId;
    public Governorate? Governorate { get; private set; }


}
