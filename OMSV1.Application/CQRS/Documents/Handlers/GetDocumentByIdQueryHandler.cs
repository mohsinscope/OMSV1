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
            // Use the repository method that returns IQueryable<Document>.
        var document = await _unitOfWork.Repository<Document>()
            .GetAllAsQueryable()
            .Include(d => d.CCs) // Include CCs for the root document
            .Include(d => d.ChildDocuments)
                .ThenInclude(child => child.CCs) // Include CCs for first-level child documents
            .Include(d => d.ChildDocuments)
                .ThenInclude(child => child.ChildDocuments) // For further nesting (if needed)
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (document == null)
            {
                throw new KeyNotFoundException($"Document with ID {request.Id} was not found.");
            }

            // Map the document and its nested children recursively.
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
                // Updated: Map the multiple CC recipients into a list of their identifiers.
                CCIds = doc.CCs != null ? doc.CCs.Select(cc => cc.Id).ToList() : new List<Guid>(),
                ProfileId = doc.ProfileId,
                IsReplied = doc.IsReplied,
                IsAudited = doc.IsAudited,
                Datecreated = doc.DateCreated,
                ChildDocuments = new List<DocumentDetailedDto>()
            };

            if (doc.ChildDocuments != null && doc.ChildDocuments.Any())
            {
                dto.ChildDocuments = doc.ChildDocuments.Select(child => MapDocumentToDto(child)).ToList();
            }

            return dto;
        }
    }
}
