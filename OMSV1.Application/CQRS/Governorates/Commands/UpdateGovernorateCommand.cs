using MediatR;

namespace OMSV1.Application.Commands.Governorates
{
    public class UpdateGovernorateCommand : IRequest<bool>
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Code { get; }
        public bool? IsCountry { get; set; }


        public UpdateGovernorateCommand(Guid id, string name, string code,bool? isCountry)
        {
            Id = id;
            Name = name;
            Code = code;
            IsCountry= isCountry;

        }
    }
}
