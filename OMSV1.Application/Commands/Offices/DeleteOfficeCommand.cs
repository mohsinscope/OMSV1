using MediatR;

namespace OMSV1.Application.Commands.Offices
{
    public class DeleteOfficeCommand : IRequest<bool>
    {
        public int OfficeId { get; }

        public DeleteOfficeCommand(int officeId)
        {
            OfficeId = officeId;
        }
    }
}
