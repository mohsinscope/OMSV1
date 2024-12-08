using System;
using OMSV1.Domain.Entities.Users;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Profiles;

// Domain/Entities/Profile.cs
public class Profile : Entity
{
    public int UserId { get; private set; }
    
    // Reference the interface instead of concrete implementation
    public IUserReference User { get; private set; }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    private Profile() {} // For EF Core

    public static Profile Create(
        IUserReference user, 
        string firstName, 
        string lastName)
    {
        return new Profile
        {
            Id = 0, // Default value, assuming the database generates it.
            UserId = user.Id,
            User = user,
            FirstName = firstName,
            LastName = lastName
        };
    }
}

