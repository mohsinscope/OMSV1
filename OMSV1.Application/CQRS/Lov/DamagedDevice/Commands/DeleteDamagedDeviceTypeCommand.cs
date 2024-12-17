using MediatR;

namespace OMSV1.Application.Commands.LOV
{
    public class DeleteDamagedDeviceTypeCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
