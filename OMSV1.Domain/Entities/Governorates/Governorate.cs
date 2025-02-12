using System;
using System.Collections.Generic;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Governorates
{
    public class Governorate : Entity
    {
        // Parameterized constructor for domain logic.
        // Note that 'isCountry' is now of type bool? to allow null values.
        public Governorate(string name, string code, bool? isCountry)
        {
            Name = name;
            Code = code;
            IsCountry = isCountry;
        }

        // Parameterless (or protected) constructor for EF Core.
        protected Governorate()
        {
            Name = string.Empty;
            Code = string.Empty;
        }

        public string Name { get; private set; }
        public string Code { get; private set; }

        // New nullable property.
        public bool? IsCountry { get; private set; }

        private List<Office> _offices = new List<Office>();
        public IReadOnlyCollection<Office> Offices => _offices.AsReadOnly();

        // Optional: A method to update the IsCountry property.
        public void UpdateIsCountry(bool? isCountry)
        {
            IsCountry = isCountry;
        }
    }
}
