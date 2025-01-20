using System;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.DamagedDevices;

   public class DeviceType : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        // Constructor to initialize the object
        public DeviceType(string name, string description)
        {
            Name = name;
            Description = description;
        }
     // Update method to allow modification of properties
        public void Update(string? name, string? description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

}
