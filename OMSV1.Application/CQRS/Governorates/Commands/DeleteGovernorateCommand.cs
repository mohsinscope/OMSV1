using MediatR;

namespace OMSV1.Application.Commands.Governorates
{
    public class DeleteGovernorateCommand : IRequest<bool>
    {
        public int Id { get; }

        public DeleteGovernorateCommand(int id)
        {
            Id = id;
        }
    }
}
