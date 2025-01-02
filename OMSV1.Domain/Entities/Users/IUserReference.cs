using System;

namespace OMSV1.Domain.Entities.Users;

public interface IUserReference
{
    Guid Id { get; }
    string Email { get; }
    string UserName { get; }
}
