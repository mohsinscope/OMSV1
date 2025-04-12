using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
            // Retrieve the DocumentParty from the repository
            var documentParty = await _unitOfWork.Repository<DocumentParty>().GetByIdAsync(request.Id);
            if (documentParty == null)
            {
                throw new KeyNotFoundException($"Document Party with ID {request.Id} was not found.");
            }

            // Map the entity to the DTO
            return new DocumentPartyDto
            {
                Id = documentParty.Id,
                Name = documentParty.Name
            };
        }
    }
}
