using MediatR;

namespace OMSV1.Application.Queries.Thresholds
{
    public class GetThresholdByIdQuery : IRequest<ThresholdDto>
    {
        public Guid Id { get; set; }
    }
}
