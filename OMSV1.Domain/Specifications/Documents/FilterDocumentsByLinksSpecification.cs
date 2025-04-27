// Domain/Specifications/Documents/FilterDocumentsByLinksSpecification.cs
using OMSV1.Domain.Entities.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OMSV1.Domain.Specifications.Documents
{
    public class FilterDocumentsByLinksSpecification : BaseSpecification<Document>
    {
        public FilterDocumentsByLinksSpecification(
            IEnumerable<Guid>? ccIds,
            IEnumerable<Guid>? tagIds)
            : base(BuildPredicate(ccIds, tagIds))
        {
            // order by newest
            ApplyOrderByDescending(x => x.DocumentDate);
        }

        private static Expression<Func<Document, bool>> BuildPredicate(
            IEnumerable<Guid>? ccIds,
            IEnumerable<Guid>? tagIds)
        {
            return doc =>
                // if no CC filters, include all; otherwise any link matches
                (ccIds == null || !ccIds.Any()
                    || doc.CcLinks.Any(cl => ccIds.Contains(cl.DocumentCcId)))
            &&  // AND
                (tagIds == null || !tagIds.Any()
                    || doc.TagLinks.Any(tl => tagIds.Contains(tl.TagId)));
        }
    }
}
