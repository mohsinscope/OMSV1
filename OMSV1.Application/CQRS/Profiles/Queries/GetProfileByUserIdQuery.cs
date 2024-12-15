using MediatR;
using OMSV1.Application.Dtos.Profiles;

namespace OMSV1.Application.Queries.Profiles
{
    public class GetProfileByUserIdQuery : IRequest<ProfileDto>
    {
        public int UserId { get; }

        public GetProfileByUserIdQuery(int userId)
        {
            UserId = userId;
        }
    }
}
