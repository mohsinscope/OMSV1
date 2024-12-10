using MediatR;
using OMSV1.Domain.Entities.DamagedDevices;

namespace OMSV1.Application.Queries.DamagedDevices
{
    public class GetDamagedDeviceByIdQuery : IRequest<DamagedDevice?>
    {
        public int Id { get; }

        public GetDamagedDeviceByIdQuery(int id)
        {
            Id = id;
        }
    }
}
