using MediatR;

namespace OMSV1.Application.Commands.LOV
{
    public class DeleteDamagedDeviceTypeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
