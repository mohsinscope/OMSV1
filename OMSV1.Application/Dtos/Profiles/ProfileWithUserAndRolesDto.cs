using System;

namespace OMSV1.Application.Dtos.Profiles;

public class ProfileWithUserAndRolesDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Position { get; set; }
    public Guid GovernorateId { get; set; }
    public string GovernorateName { get; set; }
    public Guid OfficeId { get; set; }
    public string OfficeName { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public List<string> Roles { get; set; }
}