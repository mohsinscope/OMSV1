// Ministry.cs
using OMSV1.Domain.Entities.GeneralDirectorates;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Directorates
{
    /// <summary>
    /// Represents a government ministry associated with a document.
    /// </summary>
public class Directorate : Entity
{
    public string Name { get; private set; }
    public Guid GeneralDirectorateId { get; private set; }
    public GeneralDirectorate GeneralDirectorate { get; private set; } = null!;
    public ICollection<Department> Departments { get; private set; }

    protected Directorate()
    {
        Departments = new List<Department>();
    }

    public Directorate(string name, Guid generalDirectorateId) : this()
    {
        Name = name;
        GeneralDirectorateId = generalDirectorateId;
    }
    

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        Name = name;
    }
}
}
