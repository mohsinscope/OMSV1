using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Queries.Documents
{
    public class GetDocumentByDocumentNumberDetailedQueryHandler : IRequestHandler<GetDocumentByDocumentNumberDetailedQuery, DocumentDetailedDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDocumentByDocumentNumberDetailedQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DocumentDetailedDto> Handle(GetDocumentByDocumentNumberDetailedQuery request, CancellationToken cancellationToken)
        {
            // Get the document using the repository's GetAllAsQueryable, filtering by document number.
            // We eagerly load ChildDocuments and their ChildDocuments using Include/ThenInclude.
            var document = await _unitOfWork.Repository<Document>()
                .GetAllAsQueryable()
                .Where(d => d.DocumentNumber == request.DocumentNumber)
                .Include(d => d.ChildDocuments)
                    .ThenInclude(child => child.ChildDocuments)
                .SingleOrDefaultAsync(cancellationToken);

            if (document == null)
            {
                throw new KeyNotFoundException($"Document with DocumentNumber {request.DocumentNumber} was not found.");
            }

            // Map the document recursively.
            return MapDocumentToDto(document);
        }

        private DocumentDetailedDto MapDocumentToDto(Document doc)
        {
            var dto = new DocumentDetailedDto
            {
                Id = doc.Id,
                DocumentNumber = doc.DocumentNumber,
                Title = doc.Title,
                DocumentType = doc.DocumentType,
                ResponseType = doc.ResponseType,
                Subject = doc.Subject,
                IsRequiresReply = doc.IsRequiresReply,
                ProjectId = doc.ProjectId,
                DocumentDate = doc.DocumentDate,
                PartyId = doc.PartyId,
                CCId = doc.CCId,
                ProfileId = doc.ProfileId,
                IsReplied = doc.IsReplied,
                IsAudited = doc.IsAudited,
                Datecreated = doc.DateCreated,
                ChildDocuments = new System.Collections.Generic.List<DocumentDetailedDto>()
            };

            if (doc.ChildDocuments != null && doc.ChildDocuments.Any())
            {
                dto.ChildDocuments = doc.ChildDocuments.Select(child => MapDocumentToDto(child)).ToList();
            }

            return dto;
        }
    }
}
