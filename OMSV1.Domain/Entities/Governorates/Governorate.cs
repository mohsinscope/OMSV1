using System;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Governorates;

public class Governorate(string name, string code) : Entity
{
    public string Name { get; private set; } = name;
    public string Code { get; private set; } = code;
    private List<Office> _offices = new List<Office>();
    public IReadOnlyCollection<Office> Offices => _offices.AsReadOnly();

}
