using MediatR;

namespace OMSV1.Application.Commands.Governorates
{
    public class AddGovernorateCommand : IRequest<Guid>
    {
        public string Name { get; }
        public string Code { get; }

        public AddGovernorateCommand(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}
