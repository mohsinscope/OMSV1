// Domain/Specifications/Documents/FilterDocumentsSpecification.cs
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Enums;

namespace OMSV1.Domain.Specifications.Documents
{
    public class FilterDocumentsSpecification : BaseSpecification<Document>
    {
        public FilterDocumentsSpecification(
            string? documentNumber   = null,
            DateTime? documentDate   = null,
            string? title            = null,
            string? subject          = null,
            DocumentType? documentType   = null,
            ResponseType? responseType   = null,
            bool? isRequiresReply    = null,
            bool? isReplied          = null,
            bool? isAudited          = null,
            bool? isUrgent           = null,
            bool? isImportant        = null,
            bool? isNeeded           = null,
            string? notes            = null,
            Guid? projectId          = null,
            Guid? partyId            = null,
            Guid? ministryId         = null,
            Guid? parentDocumentId   = null,
            Guid? profileId          = null
        ) : base(x =>
            (string.IsNullOrEmpty(documentNumber)   || x.DocumentNumber.Contains(documentNumber!)) &&
            (!documentDate.HasValue                || x.DocumentDate.Date == documentDate.Value.Date) &&
            (string.IsNullOrEmpty(title)            || x.Title.Contains(title!)) &&
            (string.IsNullOrEmpty(subject)          || x.Subject!.Contains(subject!)) &&
            (!documentType.HasValue                || x.DocumentType == documentType.Value) &&
            (!responseType.HasValue                || x.ResponseType == responseType.Value) &&

            (!isRequiresReply.HasValue             || x.IsRequiresReply == isRequiresReply.Value) &&
            (!isReplied.HasValue                   || x.IsReplied == isReplied.Value) &&
            (!isAudited.HasValue                   || x.IsAudited == isAudited.Value) &&

            (!isUrgent.HasValue                    || x.IsUrgent == isUrgent.Value) &&
            (!isImportant.HasValue                 || x.IsImportant == isImportant.Value) &&
            (!isNeeded.HasValue                    || x.IsNeeded == isNeeded.Value) &&

            (string.IsNullOrEmpty(notes)            || x.Notes!.Contains(notes!)) &&

            (!projectId.HasValue                   || x.ProjectId == projectId.Value) &&
            (!partyId.HasValue                     || x.PartyId == partyId.Value) &&
            (!ministryId.HasValue                  || x.MinistryId == ministryId.Value) &&
            (!parentDocumentId.HasValue            || x.ParentDocumentId == parentDocumentId.Value) &&
            (!profileId.HasValue                   || x.ProfileId == profileId.Value)
        )
        {
            // Order by newest first
            ApplyOrderByDescending(x => x.DocumentDate);
        }
    }
}
