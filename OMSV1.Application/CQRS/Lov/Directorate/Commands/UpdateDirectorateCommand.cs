// Application/Commands/Directorate/UpdateDirectorateCommand.cs
using MediatR;

namespace OMSV1.Application.Commands.Directorates
{
    public class UpdateDirectorateCommand : IRequest<bool>
    {
        public Guid Id { get; }
        public string Name { get; }


        public UpdateDirectorateCommand(Guid id, string name)
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
