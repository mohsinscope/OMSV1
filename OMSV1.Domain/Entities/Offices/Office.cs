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
                    Guid GovernorateId) : Entity
{
    public string Name { get; private set; } = name;
    public int Code { get; private set; } = code;
    public int ReceivingStaff { get; private set; } = receivingStaff;
    public int AccountStaff { get; private set; } = accountStaff;
    public int PrintingStaff { get; private set; } = printingStaff;
    public int QualityStaff { get; private set; } = qualityStaff;
    public int DeliveryStaff { get; private set; } = deliveryStaff;
    public Guid GovernorateId { get; private set; } = GovernorateId;
    public Governorate? Governorate { get; private set; }

    public void UpdateCode(int code)
    {
        throw new NotImplementedException();
    }

    public void UpdateName(string name)
    {
        throw new NotImplementedException();
    }

    public void UpdateStaff(int receivingStaff, int accountStaff, int printingStaff, int qualityStaff, int deliveryStaff)
    {
        throw new NotImplementedException();
    }
}
