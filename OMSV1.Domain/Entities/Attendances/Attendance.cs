using OMSV1.Domain.Entities.Governorates;

using OMSV1.Domain.Entities.Offices;

using OMSV1.Domain.SeedWork;

using OMSV1.Domain.Enums;

using OMSV1.Domain.Entities.Profiles;

namespace OMSV1.Domain.Entities.Attendances;

  

public class Attendance(

                     int receivingStaff,

                     int accountStaff,

                     int printingStaff,
                     
                     int qualityStaff,

                     int deliveryStaff,

                     DateTime date,

                     string note,

                     WorkingHours workingHours,

                     Guid governorateId,

                     Guid officeId,

                     Guid profileId) : Entity

{

     public int ReceivingStaff { get; private set; } = receivingStaff;

     public int AccountStaff { get; private set; } = accountStaff;

    public int PrintingStaff { get; private set; } = printingStaff;

     public int QualityStaff { get; private set; } = qualityStaff;

     public int DeliveryStaff { get; private set; } = deliveryStaff;

     public DateTime Date { get; private set; } = date;

     public string Note { get; private set; } = note;

     public WorkingHours WorkingHours { get; private set; } = workingHours;

    public Guid OfficeId { get; private set; } = officeId;

     public Guid GovernorateId { get; private set; } = governorateId;

     public Guid ProfileId { get; private set; } = profileId;

  
  

     public Governorate Governorate { get; private set; }= null!;

    public Office Office { get; private set; } = null!;

     public Profile Profile { get; private set; }= null!;

       // Method to update attendance details

     // Method to update attendance details

    public void UpdateDetails(

        int receivingStaff,

        int accountStaff,

        int printingStaff,

        int qualityStaff,

        int deliveryStaff,

        DateTime date,

        string note,

        WorkingHours workingHours,

        Guid governorateId,

        Guid officeId,

        Guid profileId)

    {

        ReceivingStaff = receivingStaff;

        AccountStaff = accountStaff;

        PrintingStaff = printingStaff;

        QualityStaff = qualityStaff;

        DeliveryStaff = deliveryStaff;

        Date = date;

        Note = note;

        WorkingHours = workingHours;

        GovernorateId = governorateId;

        OfficeId = officeId;

        ProfileId = profileId;

    }

  
     public void UpdateDate(DateTime date)
    {
        Date = date;
    }

}