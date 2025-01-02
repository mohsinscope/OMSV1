using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.CQRS.Users.Commands;
public class UpdateUserCommand : IRequest<IActionResult>
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public Position Position { get; set; }
    public Guid? OfficeId { get; set; }
    public Guid? GovernorateId { get; set; }
    public IEnumerable<string> Roles { get; set; }
}