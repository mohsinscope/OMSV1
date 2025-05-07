// Application/Commands/GeneralDirectorate/UpdateGeneralDirectorateCommand.cs
using MediatR;

namespace OMSV1.Application.Commands.GeneralDirectorates
{
    public class UpdateGeneralDirectorateCommand : IRequest<bool>
    {
        public Guid Id { get; }
        public string Name { get; }


        public UpdateGeneralDirectorateCommand(Guid id, string name)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            Id = id;
            Name = name;
        }
    }
}
