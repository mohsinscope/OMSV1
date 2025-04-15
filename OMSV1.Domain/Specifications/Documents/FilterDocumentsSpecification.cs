using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Enums;


namespace OMSV1.Domain.Specifications.Documents
{
    public class FilterDocumentsSpecification : BaseSpecification<Document>
    {
        public FilterDocumentsSpecification(
            string? documentNumber = null,
            DateTime? documentDate = null,
            string? title = null,
            DocumentType? documentType = null,
            ResponseType? responseType = null,
            Guid? partyId = null,
            bool? isAudited = null,
            bool? isReplied = null,
            bool? isRequiresReply = null
        ) : base(x =>
            (string.IsNullOrEmpty(documentNumber) || x.DocumentNumber.Contains(documentNumber)) &&
            (!documentDate.HasValue || x.DocumentDate.Date == documentDate.Value.Date) &&
            (string.IsNullOrEmpty(title) || x.Title.Contains(title)) &&
            (!documentType.HasValue || x.DocumentType == documentType.Value) &&
            (!responseType.HasValue || x.ResponseType == responseType.Value) &&
            (!partyId.HasValue || x.PartyId == partyId.Value) &&
            (!isAudited.HasValue || x.IsAudited == isAudited.Value) &&
            (!isReplied.HasValue || x.IsReplied == isReplied.Value) &&
            (!isRequiresReply.HasValue || x.IsRequiresReply == isRequiresReply.Value)
        )
        {
            // Example: Order the documents by DocumentDate descending.
            ApplyOrderByDescending(x => x.DocumentDate);
        }
    }
}
