using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Enums;
using OMSV1.Domain.Entities.Profiles;
using System;

namespace OMSV1.Domain.Entities.Attendances
{
    public class Attendance : Entity
    {
        // Constructor with required parameters
        public Attendance(
            int receivingStaff,
            int accountStaff,
            int printingStaff,
            int qualityStaff,
            int deliveryStaff,
            DateTime date,
            string note,
            WorkingHours workingHours,
            int governorateId,
            int officeId,
            int profileId)
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

        public int ReceivingStaff { get; private set; }
        public int AccountStaff { get; private set; }
        public int PrintingStaff { get; private set; }
        public int QualityStaff { get; private set; }
        public int DeliveryStaff { get; private set; }
        public DateTime Date { get; private set; }
        public string Note { get; private set; }
        public WorkingHours WorkingHours { get; private set; }
        public int OfficeId { get; private set; }
        public int GovernorateId { get; private set; }
        public int ProfileId { get; private set; }

        public Governorate Governorate { get; private set; }
        public Office Office { get; private set; }
        public Profile Profile { get; private set; }

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
            int governorateId,
            int officeId,
            int profileId)
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
}
