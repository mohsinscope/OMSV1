using MediatR;

namespace OMSV1.Application.Commands.DamagedPassports
{
    public class DeleteDamagedPassportCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteDamagedPassportCommand(int id)
        {
            Id = id;
        }
    }
}
