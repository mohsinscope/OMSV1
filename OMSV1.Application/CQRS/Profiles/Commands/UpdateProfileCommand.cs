using MediatR;
using OMSV1.Application.Dtos.Profiles;

namespace OMSV1.Application.CQRS.Queries.Profiles;

public class UpdateProfileCommand : IRequest<ProfileDto>
{
    public Guid UserId { get; set; }

    public Guid ProfileId { get; set; }
    public required string FullName { get; set; }
    public required string Position { get; set; }
    public Guid OfficeId { get; set; }
    public Guid GovernorateId { get; set; }
}