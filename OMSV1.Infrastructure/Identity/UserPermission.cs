namespace OMSV1.Infrastructure.Identity;

public class UserPermission
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public string Permission { get; set; } = string.Empty; // e.g., "DamagedDevice:create"
}
