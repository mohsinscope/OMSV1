using MediatR;
using OMSV1.Application.Dtos;
namespace OMSV1.Application.Queries.LOV
{
    public class GetDamagedDeviceTypeQuery : IRequest<DamagedDeviceTypeDto>
    {
        public Guid Id { get; set; }
    }
}
