using MediatR;

namespace OMSV1.Application.Queries.Profiles
{
    public class GetProfileIdByUserIdQuery : IRequest<int>
    {
        public string UserName { get; }

        public GetProfileIdByUserIdQuery(string userName)
        {
            userName = UserName;
        }
    }
}
