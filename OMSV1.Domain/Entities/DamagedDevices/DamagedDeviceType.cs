using System;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.DamagedDevices
{
    public class DamagedDeviceType : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        // Constructor to initialize the object
        public DamagedDeviceType(string name, string description)
        {
            Name = name;
            Description = description;
        }

        // You can add more methods or validation logic here as needed.
        // Update method to modify the properties
        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
