using MediatR;
using OMSV1.Application.Dtos;
using System.Collections.Generic;

namespace OMSV1.Application.Commands.LOV
{
    public class GetAllDamagedDeviceTypesQuery : IRequest<List<DamagedDeviceTypeDto>>
    {
    }
}
