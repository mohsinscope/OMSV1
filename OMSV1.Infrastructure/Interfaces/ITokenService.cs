using System;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Infrastructure.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(ApplicationUser user);

}