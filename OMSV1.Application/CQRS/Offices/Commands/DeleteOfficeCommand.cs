using MediatR;

namespace OMSV1.Application.Commands.Offices
{
    public class DeleteOfficeCommand : IRequest<bool>
    {
        public Guid OfficeId { get; }

        public DeleteOfficeCommand(Guid officeId)
        {
            OfficeId = officeId;
        }
    }
}
