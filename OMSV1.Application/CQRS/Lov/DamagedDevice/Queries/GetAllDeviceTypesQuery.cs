using MediatR;
using OMSV1.Application.Dtos.LOV;
using OMSV1.Application.Helpers; // For PaginationParams and PagedList

namespace OMSV1.Application.CQRS.Lov.DamagedDevice
{
    // Change return type from List<DeviceTypeDto> to PagedList<DeviceTypeDto>
    public class GetAllDeviceTypesQuery : IRequest<PagedList<DeviceTypeDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllDeviceTypesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
