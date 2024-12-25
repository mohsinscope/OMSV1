using MediatR;
using OMSV1.Application.Dtos.DamagedDevices;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.DamagedDevices
{
    public class GetAllDamagedDevicesQuery : IRequest<PagedList<DamagedDeviceAllDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllDamagedDevicesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
