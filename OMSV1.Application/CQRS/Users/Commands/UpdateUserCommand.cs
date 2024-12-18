using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.CQRS.Users.Commands;

public class UpdateUserCommand : IRequest<IActionResult>
{
    public int UserId { get; set; } // Changed to int
    public string UserName { get; set; }
    public List<string> Roles { get; set; } = new();
}
