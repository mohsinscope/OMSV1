// --- GetDocumentHistoryByDocumentIdQueryHandler.cs ---
using MediatR;
using Microsoft.EntityFrameworkCore; // for ToListAsync
using OMSV1.Application.Dtos.Documents;
using OMSV1.Domain.Entities.DocumentHistories;
using OMSV1.Domain.SeedWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Queries.Documents
{
    public class GetDocumentHistoryByDocumentIdQueryHandler 
        : IRequestHandler<GetDocumentHistoryByDocumentIdQuery, List<DocumentHistoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDocumentHistoryByDocumentIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DocumentHistoryDto>> Handle(
            GetDocumentHistoryByDocumentIdQuery request, 
            CancellationToken cancellationToken)
        {
            // 1) Eager-load Profile
            var histories = await _unitOfWork.Repository<DocumentHistory>()
                .GetAllAsQueryable()
                .Where(h => h.DocumentId == request.DocumentId)
                .Include(h => h.Profile)
                .OrderBy(h => h.ActionDate)
                .ToListAsync(cancellationToken);

            if (!histories.Any())
                throw new KeyNotFoundException(
                    $"No history found for Document ID '{request.DocumentId}'.");

            // 2) Map safely
            var dtos = histories.Select(h => new DocumentHistoryDto
            {
                Id               = h.Id,
                DocumentId       = h.DocumentId,
                ActionType       = (int)h.ActionType,
                ProfileId        = h.ProfileId,
                ProfileFullName  = h.Profile?.FullName ?? string.Empty,
                ActionDate       = h.ActionDate,
                Notes            = h.Notes,
                Datecreated      = h.DateCreated
            })
            .ToList();

            return dtos;
        }
    }
}
