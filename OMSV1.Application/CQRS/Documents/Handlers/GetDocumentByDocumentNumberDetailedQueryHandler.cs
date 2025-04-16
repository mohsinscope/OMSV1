using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            // Load all documents including the necessary related data.
            var allDocuments = await _unitOfWork.Repository<Document>()
                .GetAllAsQueryable()
                .Include(d => d.Profile) // Load the Profile (to map ProfileFullName)
                .Include(d => d.CCs)     // Load CCs for all documents.
                .ToListAsync(cancellationToken);

            // Identify the root document using the provided DocumentNumber.
            var rootDocument = allDocuments.FirstOrDefault(d => d.DocumentNumber == request.DocumentNumber);
            if (rootDocument == null)
            {
                throw new KeyNotFoundException($"Document with DocumentNumber {request.DocumentNumber} was not found.");
            }

            // Build and return the full recursive tree.
            return BuildDocumentTree(rootDocument, allDocuments);
        }

        /// <summary>
        /// Recursively builds the document tree from the flat list of documents.
        /// </summary>
        private DocumentDetailedDto BuildDocumentTree(Document doc, IEnumerable<Document> allDocs)
        {
            // Map the current document.
            var dto = MapDocumentToDto(doc);
            
            // Find all immediate child documents.
            var children = allDocs.Where(x => x.ParentDocumentId == doc.Id).ToList();
            foreach (var child in children)
            {
                dto.ChildDocuments.Add(BuildDocumentTree(child, allDocs));
            }

            return dto;
        }

        /// <summary>
        /// Maps a single Document to its DTO.
        /// </summary>
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
                // Map CC recipients into a nullable list of their IDs.
                CCIds = (doc.CCs != null && doc.CCs.Any()) ? doc.CCs.Select(cc => cc.Id).ToList() : null,
                ProfileId = doc.ProfileId,
                ProfileFullName = doc.Profile?.FullName,
                IsReplied = doc.IsReplied,
                IsAudited = doc.IsAudited,
                Datecreated = doc.DateCreated,
                Notes = doc.Notes,
                ChildDocuments = new List<DocumentDetailedDto>()
            };

            return dto;
        }
    }
}
