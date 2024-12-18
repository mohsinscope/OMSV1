using System;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Users;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Profiles;

 public class Profile : Entity,IAggregateRoot
    {
    private Profile() { }

    public Profile(int userId, string fullName,  Position position,int officeId,int governorateId)
    {
        UserId = userId;
        FullName = fullName;
        Position = position;
        OfficeId = officeId;
        GovernorateId = governorateId;
    }
        public string FullName { get; private set; }
        public Position Position { get; private set; }
        public int UserId { get; private set; }
        public int OfficeId { get; private set; } 
        public int GovernorateId { get; private set; } 
        public Office? Office { get; private set; }      
        public Governorate? Governorate { get; private set; }
        
  




    public void UpdateProfile(string fullName, Position position, int officeId, int governorateId)
    {
        FullName = fullName;
        Position = position;
        OfficeId = officeId;
        GovernorateId = governorateId;
    }


    }


