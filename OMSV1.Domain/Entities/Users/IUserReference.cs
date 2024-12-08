using System;

namespace OMSV1.Domain.Entities.Users;

public interface IUserReference
{
    int Id { get; }
    string Email { get; }
    string UserName { get; }
}
