using System;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Expenses;

public class ExpenseType(string name) : Entity
{
    public string Name { get; private set; } = name;

}
