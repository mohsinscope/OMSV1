using MediatR;
namespace OMSV1.Application.CQRS.Profiles.Queries;

public class GetAllRolesQuery : IRequest<List<string>> { }
