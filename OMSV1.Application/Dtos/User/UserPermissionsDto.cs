namespace OMSV1.Application.CQRS.Queries.Users
{
    public class UserPermissionsDto
    {
        public List<string> Roles { get; set; } = new List<string>();
        public Dictionary<string, List<string>> Permissions { get; set; } = new Dictionary<string, List<string>>();
    }
}
