using System;
using MediatR;
using OMSV1.Application.Dtos.Profiles;

namespace OMSV1.Application.CQRS.Queries.Profiles;

public class GetProfilesWithUsersAndRolesQuery : IRequest<List<ProfileWithUserAndRolesDto>>
{
}