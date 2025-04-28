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
    public class GetDocumentByIdDetailedQueryHandler 
        : IRequestHandler<GetDocumentByIdDetailedQuery, DocumentDetailedDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDocumentByIdDetailedQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DocumentDetailedDto> Handle(
            GetDocumentByIdDetailedQuery request, 
            CancellationToken cancellationToken)
        {
            // 1) Load all documents with required nav props, including tag links
            var allDocuments = await _unitOfWork.Repository<Document>()
                .GetAllAsQueryable()
                .Include(d => d.Profile)
                .Include(d => d.CcLinks)
                    .ThenInclude(link => link.DocumentCc)
                .Include(d => d.TagLinks)
                    .ThenInclude(link => link.Tag)
                .Include(d => d.Party)
                .Include(d => d.Ministry)
                .Include(d => d.ChildDocuments)
                    .ThenInclude(cd => cd.TagLinks)
                        .ThenInclude(link => link.Tag)
                .ToListAsync(cancellationToken);

            // 2) Find target document
            var root = allDocuments.FirstOrDefault(d => d.Id == request.Id);
            if (root == null)
                throw new KeyNotFoundException($"Document with ID {request.Id} was not found.");

            // 3) Build DTO tree
            return BuildDocumentTree(root, allDocuments);
        }

        private DocumentDetailedDto BuildDocumentTree(Document doc, IEnumerable<Document> allDocs)
        {
            var dto = MapDocumentToDto(doc);

            // recurse children
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
                Id               = doc.Id,
                DocumentNumber   = doc.DocumentNumber,
                Title            = doc.Title,
                DocumentType     = doc.DocumentType,
                ResponseType     = doc.ResponseType,
                Subject          = doc.Subject,

                // flags
                IsRequiresReply  = doc.IsRequiresReply,
                IsReplied        = doc.IsReplied,
                IsAudited        = doc.IsAudited,
                IsUrgent         = doc.IsUrgent,
                IsImportant      = doc.IsImportant,
                IsNeeded         = doc.IsNeeded,

                // dates & notes
                DocumentDate     = doc.DocumentDate,
                DateCreated      = doc.DateCreated,
                Notes            = doc.Notes,

                // hierarchy
                ParentDocumentId = doc.ParentDocumentId,

                // FKs & nav
                ProjectId        = doc.ProjectId,
                MinistryId       = doc.MinistryId,
                MinistryName     = doc.Ministry?.Name,

                PartyId          = doc.PartyId,
                PartyName        = doc.Party.Name,
                PartyType        = doc.Party.PartyType,
                PartyIsOfficial  = doc.Party.IsOfficial,

               // CC & Tag ids + names
        CcIds   = doc.CcLinks.Select(l => l.DocumentCcId).ToList(),
        CcNames = doc.CcLinks.Select(l => l.DocumentCc.RecipientName!)
                              .Where(n => n != null).ToList(),
        TagIds  = doc.TagLinks.Select(l => l.TagId).ToList(),
        TagNames= doc.TagLinks.Select(l => l.Tag.Name!)
                              .Where(n => n != null).ToList(),
                // profile
                ProfileId        = doc.ProfileId,
                ProfileFullName  = doc.Profile.FullName,

                // initialize children
                ChildDocuments   = new List<DocumentDetailedDto>()
            };
        }
    }
}
