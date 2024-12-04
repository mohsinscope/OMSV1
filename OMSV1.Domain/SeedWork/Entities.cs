using System;

namespace OMSV1.Domain.SeedWork;

public class Entity
{
    public int Id { get; protected set; }
    public DateTime DateCreated { get; protected set;} = DateTime.UtcNow;

}
