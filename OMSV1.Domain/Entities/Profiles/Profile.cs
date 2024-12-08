using System;
using OMSV1.Domain.Entities.Users;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Profiles;

 public class Profile : IAggregateRoot
    {
        public Guid Id { get; private set; }

        public string UserId { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }

        private Profile() { }

        public Profile(string userId, string firstName, string lastName, DateTime dateOfBirth)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

        public void UpdateName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public void UpdateDateOfBirth(DateTime dateOfBirth)
        {
            DateOfBirth = dateOfBirth;
        }
    }


