using MediatR;
using OMSV1.Application.Dtos.DamagedDevices;
namespace OMSV1.Application.CQRS.Commands.DamagedDevices;

public class GetDamagedDevicesByGovernorateQuery : IRequest<List<DamagedDeviceDto>>
{
    public int GovernorateId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

