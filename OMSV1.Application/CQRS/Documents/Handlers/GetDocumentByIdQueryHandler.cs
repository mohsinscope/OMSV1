using AutoMapper;
using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            // Use the repository method that accepts includes.
            // Ensure that your repository implementation of GetByIdWithIncludesAsync eagerly loads the ChildDocuments.
            var document = await _unitOfWork.Repository<Document>().GetByIdWithIncludesAsync(
                request.Id, 
                d => d.ChildDocuments
            );
            
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
                Subject = doc.Subject,
                IsRequiresReply = doc.IsRequiresReply,
                ProjectId = doc.ProjectId,
                DocumentDate = doc.DocumentDate,
                PartyId = doc.PartyId,
                CCId = doc.CCId,
                Datecreated=doc.DateCreated,
                ChildDocuments = new List<DocumentDetailedDto>()
            };

            // Recursively map child documents.
            if (doc.ChildDocuments != null && doc.ChildDocuments.Any())
            {
                dto.ChildDocuments = doc.ChildDocuments.Select(child => MapDocumentToDto(child)).ToList();
            }

            return dto;
        }
    }
}
