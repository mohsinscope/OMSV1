using System;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.DamagedPassport
{
    public class DamagedType : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public DamagedType(string name, string description)
        {
            Name = name;
            Description = description;
        }
            // Update method to modify the properties
        public void Update(string? name, string? description)
        {
            Name = name?? Name;
            Description = description?? Description;
        }
    }
}
