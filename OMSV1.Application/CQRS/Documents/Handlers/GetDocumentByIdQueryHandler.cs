// --- GetDocumentByIdDetailedQueryHandler.cs ---
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
            var doc = await _unitOfWork.Repository<Document>()
                .GetAllAsQueryable()
                // **Make sure we load Project**
                .Include(d => d.Project)
                // Full hierarchy under Section
                .Include(d => d.Section)
                    .ThenInclude(s => s.Department)
                        .ThenInclude(dep => dep.Directorate)
                            .ThenInclude(dir => dir.GeneralDirectorate)
                                .ThenInclude(gd => gd.Ministry)
                .Include(d => d.PrivateParty)
                .Include(d => d.Profile)
                .Include(d => d.CcLinks).ThenInclude(l => l.DocumentCc)
                .Include(d => d.TagLinks).ThenInclude(l => l.Tag)
                .Include(d => d.ChildDocuments)
                    .ThenInclude(cd => cd.CcLinks).ThenInclude(l => l.DocumentCc)
                .Include(d => d.ChildDocuments)
                    .ThenInclude(cd => cd.TagLinks).ThenInclude(l => l.Tag)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (doc == null)
                throw new KeyNotFoundException($"Document with ID '{request.Id}' not found.");

            return BuildDocumentTree(doc);
        }

        private DocumentDetailedDto BuildDocumentTree(Document d)
        {
            var dto = MapToDetailedDto(d);

            foreach (var child in d.ChildDocuments.OrderBy(cd => cd.DocumentDate))
                dto.ChildDocuments.Add(BuildDocumentTree(child));

            return dto;
        }

        private DocumentDetailedDto MapToDetailedDto(Document d)
        {
            // Safely unwrap hierarchy
            var sec = d.Section;
            var dep = sec?.Department;
            var dir = dep?.Directorate;
            var gd  = dir?.GeneralDirectorate;
            var min = gd?.Ministry;

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
                SectionId                = sec?.Id,
                SectionName              = sec?.Name               ?? string.Empty,
                DepartmentId             = dep?.Id,
                DepartmentName           = dep?.Name              ?? string.Empty,
                DirectorateId            = dir?.Id,
                DirectorateName          = dir?.Name             ?? string.Empty,
                GeneralDirectorateId     = gd?.Id,
                GeneralDirectorateName   = gd?.Name              ?? string.Empty,
                MinistryId               = min?.Id,
                MinistryName             = min?.Name             ?? string.Empty,

                // Private party
                PrivatePartyId           = d.PrivatePartyId,
                PrivatePartyName         = d.PrivateParty?.Name   ?? string.Empty,

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
