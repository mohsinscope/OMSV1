using MediatR;
using OMSV1.Application.Dtos.Profiles;

namespace OMSV1.Application.Queries.Profiles
{
    public class GetProfileByUserIdQuery : IRequest<ProfileDto>
    {
        public Guid UserId { get; }

        public GetProfileByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
