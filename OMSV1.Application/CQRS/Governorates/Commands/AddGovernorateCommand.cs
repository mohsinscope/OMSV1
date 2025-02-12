using MediatR;

namespace OMSV1.Application.Commands.Governorates
{
    public class AddGovernorateCommand : IRequest<Guid>
    {
        public string Name { get; }
        public string Code { get; }
        public bool? IsCountry { get; set; }


        public AddGovernorateCommand(string name, string code,bool? isCountry)
        {
            Name = name;
            Code = code;
            IsCountry= isCountry;
        }
    }
}
