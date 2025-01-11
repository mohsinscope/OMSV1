using MediatR;

namespace OMSV1.Application.Queries.Thresholds
{
    public class GetThresholdsQuery : IRequest<List<ThresholdDto>>
    {
    }
}
