using MediatR;

namespace OMSV1.Application.Commands.Governorates
{
    public class UpdateGovernorateCommand : IRequest<bool>
    {
        public int Id { get; }
        public string Name { get; }
        public string Code { get; }

        public UpdateGovernorateCommand(int id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }
    }
}
