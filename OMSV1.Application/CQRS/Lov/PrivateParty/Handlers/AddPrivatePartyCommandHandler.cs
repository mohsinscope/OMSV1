// Application/Handlers/Documents/AddPrivatePartyCommandHandler.cs
using MediatR;
using OMSV1.Application.Commands.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Documents
{
    public class AddPrivatePartyCommandHandler : IRequestHandler<AddPrivatePartyCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddPrivatePartyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddPrivatePartyCommand request, CancellationToken cancellationToken)
        {
            // 1) Prevent duplicates globally
            var exists = await _unitOfWork.Repository<PrivateParty>()
                .FirstOrDefaultAsync(pp => pp.Name == request.Name);
            if (exists != null)
                throw new HandlerException($"A PrivateParty named '{request.Name}' already exists.");

            // 2) Create and persist
            var party = new PrivateParty(request.Name);
            await _unitOfWork.Repository<PrivateParty>().AddAsync(party);

            if (!await _unitOfWork.SaveAsync(cancellationToken))
                throw new HandlerException("Failed to save PrivateParty.");

            return party.Id;
        }
    }
}
