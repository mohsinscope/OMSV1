using MediatR;
using Microsoft.EntityFrameworkCore; // For ToListAsync
using OMSV1.Application.Dtos.Documents;
using OMSV1.Domain.Entities.DocumentHistories;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Queries.Documents
{
    public class GetDocumentHistoryByDocumentIdQueryHandler : IRequestHandler<GetDocumentHistoryByDocumentIdQuery, List<DocumentHistoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDocumentHistoryByDocumentIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DocumentHistoryDto>> Handle(GetDocumentHistoryByDocumentIdQuery request, CancellationToken cancellationToken)
        {
            // Use the repository's GetAllAsQueryable method to build a query that filters by DocumentId.
            var query = _unitOfWork.Repository<DocumentHistory>()
                .GetAllAsQueryable()
                .Where(h => h.DocumentId == request.DocumentId);

            // Execute the query using ToListAsync
            var histories = await query.ToListAsync(cancellationToken);

            // Map each DocumentHistory entity to a DocumentHistoryDto.
            var historyDtos = histories.Select(history => new DocumentHistoryDto
            {
                Id = history.Id,
                DocumentId = history.DocumentId,
                ActionType = (int)history.ActionType,
                UserId = history.UserId,
                ActionDate = history.ActionDate,
                Notes = history.Notes,
                Datecreated=history.DateCreated
            }).ToList();

            return historyDtos;
        }
    }
}
