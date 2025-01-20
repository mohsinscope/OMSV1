using System;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Users;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Profiles;

 public class Profile : Entity,IAggregateRoot
    {
private Profile() 
{
    FullName = string.Empty; // Default to an empty string
}

    public Profile(Guid userId, string fullName,  Position position,Guid officeId,Guid governorateId)
    {
        UserId = userId;
        FullName = fullName;
        Position = position;
        OfficeId = officeId;
        GovernorateId = governorateId;
    }
        public string  FullName { get; private set; }
        public Position Position { get; private set; }
        public Guid UserId { get; private set; }
        public Guid OfficeId { get; private set; } 
        public Guid GovernorateId { get; private set; } 
        public Office Office { get; private set; }= null!;      
        public Governorate Governorate { get; private set; }= null!;
        
  




    public void UpdateProfile(string fullName, Position position, Guid officeId, Guid governorateId)
    {
        FullName = fullName;
        Position = position;
        OfficeId = officeId;
        GovernorateId = governorateId;
    }


    }


