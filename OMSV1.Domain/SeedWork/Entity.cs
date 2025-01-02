using System;

namespace OMSV1.Domain.SeedWork;

public class Entity
{
    public Guid Id { get; protected set; }
    public DateTime DateCreated { get; protected set;} = DateTime.UtcNow;

}

