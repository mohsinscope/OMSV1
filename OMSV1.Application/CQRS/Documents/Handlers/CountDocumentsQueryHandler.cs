// Application/Queries/Documents/Handlers/CountDocumentsQueryHandler.cs
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Specifications.Documents;

namespace OMSV1.Application.Queries.Documents.Handlers
{
    public class CountDocumentsQueryHandler 
        : IRequestHandler<CountDocumentsQuery, int>
    {
        private readonly IGenericRepository<Document> _repository;

        public CountDocumentsQueryHandler(IGenericRepository<Document> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(
            CountDocumentsQuery request,
            CancellationToken cancellationToken)
        {
            // 1) build spec with new hierarchy + links
            var spec = new CountDocumentsSpecification(
                documentNumber:       request.DocumentNumber,
                documentDate:         request.DocumentDate,
                title:                request.Title,
                subject:              request.Subject,
                documentType:         request.DocumentType,
                responseType:         request.ResponseType,
                isRequiresReply:      request.IsRequiresReply,
                isReplied:            request.IsReplied,
                isAudited:            request.IsAudited,
                isUrgent:             request.IsUrgent,
                isImportant:          request.IsImportant,
                isNeeded:             request.IsNeeded,
                notes:                request.Notes,
                projectId:            request.ProjectId,
                privatePartyId:       request.PrivatePartyId,
                parentDocumentId:     request.ParentDocumentId,
                profileId:            request.ProfileId,

                // hierarchy
                sectionId:            request.SectionId,
                departmentId:         request.DepartmentId,
                directorateId:        request.DirectorateId,
                generalDirectorateId: request.GeneralDirectorateId,
                ministryId:           request.MinistryId,

                // link filters
                ccIds:                request.CcIds,
                tagIds:               request.TagIds
            );

            // 2) count via EF Core
            return await _repository
                .ListAsQueryable(spec)
                .CountAsync(cancellationToken);
        }
    }
}
