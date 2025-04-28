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
            var spec = new CountDocumentsSpecification(
                request.DocumentNumber,
                request.DocumentDate,
                request.Title,
                request.Subject,
                request.DocumentType,
                request.ResponseType,
                request.IsRequiresReply,
                request.IsReplied,
                request.IsAudited,
                request.IsUrgent,
                request.IsImportant,
                request.IsNeeded,
                request.Notes,
                request.ProjectId,
                request.PartyId,
                request.MinistryId,
                request.ParentDocumentId,
                request.ProfileId,
                request.CcIds,
                request.TagIds
            );

            // EF Core CountAsync via IQueryable
            return await _repository
                .ListAsQueryable(spec)
                .CountAsync(cancellationToken);
        }
    }
}
