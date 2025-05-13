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
            // Find the document by document number first to get its ID
            var document = await _unitOfWork.Repository<Document>()
                .GetAllAsQueryable()
                .FirstOrDefaultAsync(d => d.DocumentNumber == request.DocumentNumber, cancellationToken);

            if (document == null)
                throw new KeyNotFoundException($"Document with number '{request.DocumentNumber}' not found.");

            // 1) First, get the total count of all related documents regardless of depth
            var totalCount = await GetTotalRelatedDocumentsCount(document.Id, cancellationToken);

            // 2) Gather IDs up to requested `Depth`
            var toLoad = new List<Guid> { document.Id };
            var allIds = new HashSet<Guid>(toLoad);

            for (var level = 0; level < request.Depth; level++)
            {
                var children = await _unitOfWork.Repository<Document>()
                    .GetAllAsQueryable()
                    .Where(d => d.ParentDocumentId != null && toLoad.Contains(d.ParentDocumentId.Value))
                    .Select(d => d.Id)
                    .ToListAsync(cancellationToken);

                if (children.Count == 0) break;
                children.ForEach(id => allIds.Add(id));
                toLoad = children;
            }

            // 3) Fetch them flat with only one ChildDocuments Include
            var docs = await _unitOfWork.Repository<Document>()
                .GetAllAsQueryable()
                .Where(d => allIds.Contains(d.Id))
                .Include(d => d.Ministry)
                .Include(d => d.GeneralDirectorate)
                .Include(d => d.Directorate)
                .Include(d => d.Department)
                .Include(d => d.Section)
                .Include(d => d.Project)
                .Include(d => d.PrivateParty)
                .Include(d => d.Profile)
                .Include(d => d.CcLinks).ThenInclude(l => l.DocumentCc)
                .Include(d => d.TagLinks).ThenInclude(l => l.Tag)
                .Include(d => d.ChildDocuments)  // only first‐level nav
                .ToListAsync(cancellationToken);

            // 4) Build lookup for parent → children
            var lookup = docs.ToLookup(d => d.ParentDocumentId);

            // 5) Recursive in-memory build
            DocumentDetailedDto BuildTree(Document e, int depth)
            {
                var dto = MapToDetailedDto(e);
                dto.TotalCount = totalCount; // Set the total count of all related documents

                if (depth > 0)
                {
                    foreach (var child in lookup[e.Id].OrderBy(c => c.DocumentDate))
                        dto.ChildDocuments.Add(BuildTree(child, depth - 1));
                }

                return dto;
            }

            var root = docs.Single(d => d.Id == document.Id);
            return BuildTree(root, request.Depth);
        }

        private async Task<int> GetTotalRelatedDocumentsCount(Guid rootId, CancellationToken cancellationToken)
        {
            // This set will hold all document IDs related to the root document
            var allRelatedIds = new HashSet<Guid> { rootId };
            var toProcess = new List<Guid> { rootId };

            // Continue until no more documents to process
            while (toProcess.Count > 0)
            {
                // Get all children of the current batch
                var children = await _unitOfWork.Repository<Document>()
                    .GetAllAsQueryable()
                    .Where(d => d.ParentDocumentId != null && toProcess.Contains(d.ParentDocumentId.Value))
                    .Select(d => d.Id)
                    .ToListAsync(cancellationToken);

                // If no more children found, break out of loop
                if (children.Count == 0) break;

                // Add new found children to the set and prepare them for next iteration
                toProcess.Clear();
                foreach (var childId in children)
                {
                    if (allRelatedIds.Add(childId)) // Returns true if item was added (wasn't already in set)
                    {
                        toProcess.Add(childId);
                    }
                }
            }

            // Return the total count (root + all children at any depth)
            return allRelatedIds.Count;
        }

        private DocumentDetailedDto MapToDetailedDto(Document d)
        {
            // Safely unwrap hierarchy
            var sec = d.Section;
            var dep = d.Department ?? sec?.Department;
            var dir = d.Directorate ?? dep?.Directorate;
            var gd  = d.GeneralDirectorate ?? dir?.GeneralDirectorate;
            var min = d.Ministry ?? gd?.Ministry;

            // *** Defensively guard DocumentCc and Tag ***
            var ccNames = d.CcLinks
                .Where(l => l.DocumentCc != null)
                .Select(l => l.DocumentCc!.RecipientName ?? string.Empty)
                .ToList();

            var tagNames = d.TagLinks
                .Where(l => l.Tag != null)
                .Select(l => l.Tag!.Name ?? string.Empty)
                .ToList();

            return new DocumentDetailedDto
            {
                // Identity & core
                Id               = d.Id,
                DocumentNumber   = d.DocumentNumber,
                Title            = d.Title,
                DocumentType     = d.DocumentType,
                ResponseType     = d.ResponseType,
                Subject          = d.Subject,

                // Flags
                IsRequiresReply  = d.IsRequiresReply,
                IsReplied        = d.IsReplied,
                IsAudited        = d.IsAudited,
                IsUrgent         = d.IsUrgent,
                IsImportant      = d.IsImportant,
                IsNeeded         = d.IsNeeded, 

                // Dates & notes
                DocumentDate     = d.DocumentDate,
                Notes            = d.Notes,
                DateCreated      = d.DateCreated,

                // Document hierarchy
                ParentDocumentId = d.ParentDocumentId,
                ChildDocuments   = new List<DocumentDetailedDto>(),

                // Project
                ProjectId        = d.ProjectId,
                ProjectName      = d.Project?.Name ?? string.Empty,

                // Section → Department → Directorate → GeneralDirectorate → Ministry
                SectionId              = sec?.Id,
                SectionName            = sec?.Name ?? string.Empty,

                DepartmentId           = dep?.Id,
                DepartmentName         = dep?.Name ?? string.Empty,

                DirectorateId          = dir?.Id,
                DirectorateName        = dir?.Name ?? string.Empty,

                GeneralDirectorateId   = gd?.Id,
                GeneralDirectorateName = gd?.Name ?? string.Empty,

                MinistryId             = min?.Id,
                MinistryName           = min?.Name ?? string.Empty,
                
                // Private party
                PrivatePartyId           = d.PrivatePartyId,
                PrivatePartyName         = d.PrivateParty?.Name ?? string.Empty,

                // CCs & Tags
                CcIds    = d.CcLinks.Select(l => l.DocumentCcId).ToList(),
                CcNames  = ccNames,
                TagIds   = d.TagLinks.Select(l => l.TagId).ToList(),
                TagNames = tagNames,

                // Profile
                ProfileId       = d.ProfileId,
                ProfileFullName = d.Profile?.FullName ?? string.Empty
            };
        }
    }
}