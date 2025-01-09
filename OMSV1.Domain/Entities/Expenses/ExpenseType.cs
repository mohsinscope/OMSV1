using System;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Expenses;

public class ExpenseType : Entity
{
    public string Name { get; private set; }

    public ExpenseType(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        }
        Name = name;
    }

    // Method to update the name
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("New name cannot be empty.", nameof(newName));
        }
        Name = newName;
    }
}
