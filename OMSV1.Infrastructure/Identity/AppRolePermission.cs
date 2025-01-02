using OMSV1.Infrastructure.Identity;

public class AppRolePermission
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public AppRole Role { get; set; } = null!;
    public string Permission { get; set; } = string.Empty; // e.g., "DamagedDevice:create"
}
