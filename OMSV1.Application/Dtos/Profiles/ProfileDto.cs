using System;

namespace OMSV1.Application.Dtos.Profiles;

public class ProfileDto
{
    public int ProfileId { get; set; }
    public string FullName { get; set; }
    public string Position { get; set; }
    public int GovernorateId { get; set; }
    public string GovernorateName { get; set; }
    public int OfficeId { get; set; }
    public string OfficeName { get; set; }
    public int UserId { get; set; }
}
