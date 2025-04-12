using MediatR;

namespace OMSV1.Application.Commands.DocumentParties
{
    public class AddDocumentPartyCommand : IRequest<Guid>
    {
        public string Name { get; set; }

        public AddDocumentPartyCommand(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            
            Name = name;
        }
    }
}
