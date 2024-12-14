using System;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.DamagedPassport;

public class DamagedType(string name, string description) : Entity
{
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;

}