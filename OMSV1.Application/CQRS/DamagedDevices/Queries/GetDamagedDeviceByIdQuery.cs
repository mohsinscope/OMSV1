using MediatR;
using OMSV1.Application.Dtos.DamagedDevices;  // Ensure you're using the correct DTO

namespace OMSV1.Application.Queries.DamagedDevices
{
    public class GetDamagedDeviceByIdQuery : IRequest<DamagedDeviceDto?>  // Query now returns DamagedDeviceDto
    {
        public Guid Id { get; }

        public GetDamagedDeviceByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
