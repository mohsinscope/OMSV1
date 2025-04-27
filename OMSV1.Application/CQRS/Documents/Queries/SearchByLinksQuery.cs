// Application/Queries/Documents/SearchByLinksQuery.cs
using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Documents
{
    public class SearchByLinksQuery : IRequest<PagedList<DocumentDto>>
    {
        /// <summary>List of CC‐link IDs to filter on (matches DocumentCcId)</summary>
        public List<Guid>? CcIds { get; set; }

        /// <summary>List of Tag‐link IDs to filter on (matches TagId)</summary>
        public List<Guid>? TagIds { get; set; }

        // Paging:
        public int PageNumber { get; set; } = 1;
        public int PageSize   { get; set; } = 10;
    }
}
