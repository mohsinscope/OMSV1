using MediatR;
using OMSV1.Application.Dtos.DamagedDevices;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.CQRS.DamagedDevices.Queries;

public class GetDamagedDevicesQuery : IRequest<PagedList<DamagedDeviceDto>>
{
    public Guid? GovernorateId { get; set; }
    public string? SerialNumber { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? DeviceTypeId { get; set; }
    public Guid? DamagedDeviceTypeId { get; set; }
    public Guid? OfficeId { get; set; }
    public Guid? ProfileId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public PaginationParams PaginationParams { get;set; }

    public GetDamagedDevicesQuery(PaginationParams paginationParams)
    {
        PaginationParams = paginationParams;
    }


}
