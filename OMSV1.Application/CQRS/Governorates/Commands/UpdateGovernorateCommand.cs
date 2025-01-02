using MediatR;

namespace OMSV1.Application.Commands.Governorates
{
    public class UpdateGovernorateCommand : IRequest<bool>
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Code { get; }

        public UpdateGovernorateCommand(Guid id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }
    }
}
