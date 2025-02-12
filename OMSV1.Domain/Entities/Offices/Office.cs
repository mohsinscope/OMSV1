using System;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Offices
{
public class Office : Entity
{
    // Parameterized constructor for domain logic
    public Office(string name, int code, int receivingStaff, int accountStaff, int printingStaff,
                  int qualityStaff, int deliveryStaff, Guid governorateId, decimal? budget, bool? isEmbassy)
    {
        Name = name;
        Code = code;
        ReceivingStaff = receivingStaff;
        AccountStaff = accountStaff;
        PrintingStaff = printingStaff;
        QualityStaff = qualityStaff;
        DeliveryStaff = deliveryStaff;
        GovernorateId = governorateId;
        Budget = budget;
        IsEmbassy = isEmbassy;
    }

    // Parameterless constructor for EF Core
    private Office() 
    {
        Name = string.Empty; // Default to an empty string
        // No default is set for IsEmbassy, so it will be null.
    }

    public string Name { get; private set; }
    public int Code { get; private set; }
    public int ReceivingStaff { get; private set; }
    public int AccountStaff { get; private set; }
    public int PrintingStaff { get; private set; }
    public int QualityStaff { get; private set; }
    public int DeliveryStaff { get; private set; }
    public Guid GovernorateId { get; private set; }
    public decimal? Budget { get; private set; }
    public Governorate Governorate { get; private set; } = null!;

    // Changed to a nullable bool.
    public bool? IsEmbassy { get; private set; }

    // Methods for updating properties
    public void UpdateCode(int code) => Code = code;
    public void UpdateName(string name) => Name = name;
    public void UpdateStaff(int receivingStaff, int accountStaff, int printingStaff, int qualityStaff, int deliveryStaff)
    {
        ReceivingStaff = receivingStaff;
        AccountStaff = accountStaff;
        PrintingStaff = printingStaff;
        QualityStaff = qualityStaff;
        DeliveryStaff = deliveryStaff;
    }
    public void UpdateBudget(decimal? budget) => Budget = budget;

    // Optional: A method to update the IsEmbassy flag if needed.
    public void UpdateIsEmbassy(bool? isEmbassy) => IsEmbassy = isEmbassy;
}

}
