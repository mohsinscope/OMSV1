using System;
using OMSV1.Domain.Entities.Offices;

namespace OMSV1.Domain.Entities.Governorates;

public class Governorate(string name, string code)
{
    public string Name { get; private set; } = name;
    public string Code { get; private set; } = code;
    private List<Office> _offices = new List<Office>();
    public IReadOnlyCollection<Office> Offices => _offices.AsReadOnly();

}
