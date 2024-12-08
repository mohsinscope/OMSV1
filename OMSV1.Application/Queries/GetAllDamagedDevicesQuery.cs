using MediatR;
using OMSV1.Domain.Entities.DamagedDevices;
using System.Collections.Generic;

namespace OMSV1.Application.Queries
{
    public class GetAllDamagedDevicesQuery : IRequest<IReadOnlyList<DamagedDevice>>
    {
    }
}
