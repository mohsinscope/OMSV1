using MediatR;

namespace OMSV1.Application.Commands.Thresholds
{
    public class DeleteThresholdCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
