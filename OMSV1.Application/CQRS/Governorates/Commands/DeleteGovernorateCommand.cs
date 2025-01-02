using MediatR;

namespace OMSV1.Application.Commands.Governorates
{
    public class DeleteGovernorateCommand : IRequest<bool>
    {
        public Guid Id { get; }

        public DeleteGovernorateCommand(Guid id)
        {
            Id = id;
        }
    }
}
