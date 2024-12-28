using MediatR;
using OMSV1.Application.Dtos.DamagedDevices;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.CQRS.DamagedDevices.Queries;

public class GetDamagedDevicesQuery : IRequest<PagedList<DamagedDeviceDto>>
{
    public int? GovernorateId { get; set; }
    public string? SerialNumber { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? DeviceTypeId { get; set; }
    public int? DamagedDeviceTypeId { get; set; }
    public int? OfficeId { get; set; }
    public int? ProfileId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public PaginationParams PaginationParams { get;set; }

    public GetDamagedDevicesQuery(PaginationParams paginationParams)
    {
        PaginationParams = paginationParams;
    }


}
