using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Queries.Documents
{
    public class GetDocumentByIdDetailedQueryHandler : IRequestHandler<GetDocumentByIdDetailedQuery, DocumentDetailedDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDocumentByIdDetailedQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DocumentDetailedDto> Handle(GetDocumentByIdDetailedQuery request, CancellationToken cancellationToken)
        {
            // Option 1: If you expect only a limited tree depth, you might use chained Include/ThenInclude.
            // For arbitrary nesting, Option 2 is recommended as below.
            // -------------------------------------------------------------------
            // Option 2: Load all documents (for the scope) and then build the tree in memory.
            var allDocuments = await _unitOfWork.Repository<Document>()
                .GetAllAsQueryable()
                .Include(d => d.Profile)  // Load Profile to map ProfileFullName.
                .Include(d => d.CCs)      // Load all CCs.
                .ToListAsync(cancellationToken);

            // Find the root document with the specified Id.
            var rootDocument = allDocuments.FirstOrDefault(d => d.Id == request.Id);
            if (rootDocument == null)
            {
                throw new KeyNotFoundException($"Document with ID {request.Id} was not found.");
            }

            // Build the complete tree recursively.
            var dtoTree = BuildDocumentTree(rootDocument, allDocuments);
            return dtoTree;
        }

        /// <summary>
        /// Recursively builds the document tree from the flat list of documents.
        /// </summary>
        private DocumentDetailedDto BuildDocumentTree(Document doc, IEnumerable<Document> allDocs)
        {
            // Map the current document (without recursion).
            var dto = MapDocumentToDto(doc);
            
            // Find all child documents (immediate children)
            var children = allDocs.Where(x => x.ParentDocumentId == doc.Id).ToList();
            foreach (var child in children)
            {
                dto.ChildDocuments.Add(BuildDocumentTree(child, allDocs));
            }

            return dto;
        }

        /// <summary>
        /// Maps a single Document to its DTO. This method does NOT handle recursion.
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
                // Map the CC recipients into a list of their IDs.
                CCIds = doc.CCs != null ? doc.CCs.Select(cc => cc.Id).ToList() : new List<Guid>(),
                ProfileId = doc.ProfileId,
                ProfileFullName = doc.Profile?.FullName,
                IsReplied = doc.IsReplied,
                IsAudited = doc.IsAudited,
                Datecreated = doc.DateCreated,
                // Map the optional Notes property.
                Notes = doc.Notes,
                ChildDocuments = new List<DocumentDetailedDto>()
            };

            return dto;
        }
    }
}
