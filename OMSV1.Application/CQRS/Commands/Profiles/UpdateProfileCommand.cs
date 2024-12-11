using System;
using MediatR;
using OMSV1.Application.Dtos.Profiles;

namespace OMSV1.Application.CQRS.Queries.Profiles;

public class UpdateProfileCommand : IRequest<ProfileDto>
{
    public int ProfileId { get; set; }
    public string FullName { get; set; }
    public string Position { get; set; }
    public int OfficeId { get; set; }
    public int GovernorateId { get; set; }
}