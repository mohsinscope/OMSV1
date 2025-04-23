using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.DocumentParties
{
    public class GetDocumentPartyByIdQueryHandler : IRequestHandler<GetDocumentPartyByIdQuery, DocumentPartyDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDocumentPartyByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DocumentPartyDto> Handle(GetDocumentPartyByIdQuery request, CancellationToken cancellationToken)
        {
            var documentParty = await _unitOfWork.Repository<DocumentParty>()
                .GetByIdAsync(request.Id);

            if (documentParty == null)
                throw new KeyNotFoundException($"Document Party with ID {request.Id} was not found.");

            return new DocumentPartyDto
            {
                Id = documentParty.Id,
                Name = documentParty.Name,
                PartyType = documentParty.PartyType,
                IsOfficial = documentParty.IsOfficial,
                ProjectId = documentParty.ProjectId,
                DateCreated = documentParty.DateCreated
            };
        }
    }
}

