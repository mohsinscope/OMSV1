using MediatR;
using OMSV1.Application.Dtos;
using OMSV1.Application.Helpers; // For PaginationParams and PagedList

namespace OMSV1.Application.Commands.LOV
{
    // Change return type from List<DamagedDeviceTypeDto> to PagedList<DamagedDeviceTypeDto>
    public class GetAllDamagedDeviceTypesQuery : IRequest<PagedList<DamagedDeviceTypeDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllDamagedDeviceTypesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
