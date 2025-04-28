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
    public class GetDocumentByDocumentNumberDetailedQueryHandler 
        : IRequestHandler<GetDocumentByDocumentNumberDetailedQuery, DocumentDetailedDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDocumentByDocumentNumberDetailedQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DocumentDetailedDto> Handle(
            GetDocumentByDocumentNumberDetailedQuery request, 
            CancellationToken cancellationToken)
        {
            // Load all documents with necessary related data, including tag & CC links
            var allDocuments = await _unitOfWork.Repository<Document>()
                .GetAllAsQueryable()
                .Include(d => d.Profile)
                .Include(d => d.CcLinks)
                    .ThenInclude(link => link.DocumentCc)
                .Include(d => d.TagLinks)
                    .ThenInclude(link => link.Tag)
                .Include(d => d.Party)
                .Include(d => d.Ministry)
                // include first-level children with their tag & CC links
                .Include(d => d.ChildDocuments)
                    .ThenInclude(cd => cd.CcLinks)
                        .ThenInclude(link => link.DocumentCc)
                .Include(d => d.ChildDocuments)
                    .ThenInclude(cd => cd.TagLinks)
                        .ThenInclude(link => link.Tag)
                .ToListAsync(cancellationToken);

            // Identify the root document by its number
            var root = allDocuments
                .FirstOrDefault(d => d.DocumentNumber == request.DocumentNumber);
            if (root == null)
                throw new KeyNotFoundException(
                    $"Document with DocumentNumber '{request.DocumentNumber}' not found.");

            // Build and return the recursive DTO tree
            return BuildDocumentTree(root, allDocuments);
        }

        private DocumentDetailedDto BuildDocumentTree(
            Document doc, 
            IEnumerable<Document> allDocs)
        {
            var dto = MapDocumentToDto(doc);

            // recurse into children
            var children = allDocs.Where(d => d.ParentDocumentId == doc.Id);
            foreach (var child in children)
            {
                dto.ChildDocuments.Add(BuildDocumentTree(child, allDocs));
            }

            return dto;
        }

        private DocumentDetailedDto MapDocumentToDto(Document doc)
        {
            return new DocumentDetailedDto
            {
                Id                  = doc.Id,
                DocumentNumber      = doc.DocumentNumber,
                Title               = doc.Title,
                DocumentType        = doc.DocumentType,
                ResponseType        = doc.ResponseType,
                Subject             = doc.Subject,

                IsRequiresReply     = doc.IsRequiresReply,
                IsReplied           = doc.IsReplied,
                IsAudited           = doc.IsAudited,

                IsUrgent            = doc.IsUrgent,
                IsImportant         = doc.IsImportant,
                IsNeeded            = doc.IsNeeded,

                DocumentDate        = doc.DocumentDate,
                DateCreated      = doc.DateCreated,

                Notes               = doc.Notes,

                ParentDocumentId    = doc.ParentDocumentId,

                ProjectId           = doc.ProjectId,

                MinistryId          = doc.MinistryId,
                MinistryName        = doc.Ministry?.Name,

                PartyId             = doc.PartyId,
                PartyName           = doc.Party.Name,
                PartyType           = doc.Party.PartyType,
                PartyIsOfficial     = doc.Party.IsOfficial,

               // CC & Tag ids + names
        CcIds   = doc.CcLinks.Select(l => l.DocumentCcId).ToList(),
        CcNames = doc.CcLinks.Select(l => l.DocumentCc.RecipientName!)
                              .Where(n => n != null).ToList(),
        TagIds  = doc.TagLinks.Select(l => l.TagId).ToList(),
        TagNames= doc.TagLinks.Select(l => l.Tag.Name!)
                              .Where(n => n != null).ToList(),
                ProfileId           = doc.ProfileId,
                ProfileFullName     = doc.Profile.FullName,

                ChildDocuments      = new List<DocumentDetailedDto>()
            };
        }
    }
}