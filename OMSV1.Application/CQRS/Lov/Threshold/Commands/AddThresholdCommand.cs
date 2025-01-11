using MediatR;

namespace OMSV1.Application.Commands.Thresholds
{
    public class AddThresholdCommand : IRequest<bool>
    {
        public string Name { get; set; } = string.Empty;
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
    }
}
