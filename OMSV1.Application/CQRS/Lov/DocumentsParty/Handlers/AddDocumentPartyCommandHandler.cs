// Application/Handlers/DocumentParties/AddDocumentPartyCommandHandler.cs
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.DocumentParties;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DocumentParties
{
    public class AddDocumentPartyCommandHandler : IRequestHandler<AddDocumentPartyCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddDocumentPartyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddDocumentPartyCommand request, CancellationToken cancellationToken)
        {
            // Prevent duplicates in the same project
            var existing = await _unitOfWork.Repository<DocumentParty>()
                .FirstOrDefaultAsync(dp =>
                    dp.Name == request.Name &&
                    dp.ProjectId == request.ProjectId
                    );

            if (existing != null)
                throw new HandlerException($"A party named '{request.Name}' already exists for this project.");

            // Create new DocumentParty entity
            var documentParty = new DocumentParty(
                request.Name,
                request.PartyType,
                request.IsOfficial,
                request.ProjectId);

            await _unitOfWork.Repository<DocumentParty>()
                .AddAsync(documentParty);

            if (!await _unitOfWork.SaveAsync(cancellationToken))
                throw new HandlerException("Failed to save the DocumentParty to the database.");

            return documentParty.Id;
        }
    }
}
