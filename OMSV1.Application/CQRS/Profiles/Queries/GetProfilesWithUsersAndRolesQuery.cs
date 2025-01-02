using MediatR;
using OMSV1.Application.Dtos.Profiles;

namespace OMSV1.Application.CQRS.Profiles.Queries;

public class GetProfilesWithUsersAndRolesQuery : IRequest<List<ProfileWithUserAndRolesDto>>
{
}