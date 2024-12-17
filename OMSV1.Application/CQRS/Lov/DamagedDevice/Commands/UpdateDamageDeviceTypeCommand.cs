using MediatR;

namespace OMSV1.Application.Commands.LOV
{
    public class UpdateDamagedDeviceTypeCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
