// Domain/Specifications/Documents/CountDocumentsSpecification.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Enums;

namespace OMSV1.Domain.Specifications.Documents
{
    public class CountDocumentsSpecification : BaseSpecification<Document>
    {
        public CountDocumentsSpecification(
            string? documentNumber            = null,
            DateTime? documentDate            = null,
            string? title                     = null,
            string? subject                   = null,
            DocumentType? documentType        = null,
            ResponseType? responseType        = null,
            bool? isRequiresReply             = null,
            bool? isReplied                   = null,
            bool? isAudited                   = null,
            bool? isUrgent                    = null,
            bool? isImportant                 = null,
            bool? isNeeded                    = null,
            string? notes                     = null,
            Guid? projectId                   = null,
            Guid? privatePartyId              = null,
            Guid? parentDocumentId            = null,
            Guid? profileId                   = null,

            // new hierarchy
            Guid? sectionId                   = null,
            Guid? departmentId                = null,
            Guid? directorateId               = null,
            Guid? generalDirectorateId        = null,
            Guid? ministryId                  = null,

            // links
            IEnumerable<Guid>? ccIds          = null,
            IEnumerable<Guid>? tagIds         = null
        ) : base(BuildPredicate(
                documentNumber, documentDate, title, subject,
                documentType, responseType,
                isRequiresReply, isReplied, isAudited,
                isUrgent, isImportant, isNeeded,
                notes,
                projectId, privatePartyId, parentDocumentId, profileId,
                sectionId, departmentId, directorateId, generalDirectorateId, ministryId,
                ccIds, tagIds
            ))
        {
            // no ordering/paging for count
        }

        private static Expression<Func<Document, bool>> BuildPredicate(
            string? documentNumber,
            DateTime? documentDate,
            string? title,
            string? subject,
            DocumentType? documentType,
            ResponseType? responseType,
            bool? isRequiresReply,
            bool? isReplied,
            bool? isAudited,
            bool? isUrgent,
            bool? isImportant,
            bool? isNeeded,
            string? notes,
            Guid? projectId,
            Guid? privatePartyId,
            Guid? parentDocumentId,
            Guid? profileId,

            // hierarchy
            Guid? sectionId,
            Guid? departmentId,
            Guid? directorateId,
            Guid? generalDirectorateId,
            Guid? ministryId,

            // link filters
            IEnumerable<Guid>? ccIds,
            IEnumerable<Guid>? tagIds
        )
        {
            return doc =>
                // base filters...
                (string.IsNullOrEmpty(documentNumber) || doc.DocumentNumber.Contains(documentNumber!)) &&
                (!documentDate.HasValue            || doc.DocumentDate.Date == documentDate.Value.Date) &&
                (string.IsNullOrEmpty(title)       || doc.Title.Contains(title!)) &&
                (string.IsNullOrEmpty(subject)     || doc.Subject!.Contains(subject!)) &&
                (!documentType.HasValue            || doc.DocumentType == documentType.Value) &&
                (!responseType.HasValue            || doc.ResponseType == responseType.Value) &&

                (!isRequiresReply.HasValue         || doc.IsRequiresReply == isRequiresReply.Value) &&
                (!isReplied.HasValue               || doc.IsReplied == isReplied.Value) &&
                (!isAudited.HasValue               || doc.IsAudited == isAudited.Value) &&

                (!isUrgent.HasValue                || doc.IsUrgent == isUrgent.Value) &&
                (!isImportant.HasValue             || doc.IsImportant == isImportant.Value) &&
                (!isNeeded.HasValue                || doc.IsNeeded == isNeeded.Value) &&

                (string.IsNullOrEmpty(notes)       || doc.Notes!.Contains(notes!)) &&

                (!projectId.HasValue               || doc.ProjectId == projectId.Value) &&
                (!privatePartyId.HasValue          || doc.PrivatePartyId == privatePartyId.Value) &&
                (!parentDocumentId.HasValue        || doc.ParentDocumentId == parentDocumentId.Value) &&
                (!profileId.HasValue               || doc.ProfileId == profileId.Value) &&

                // hierarchical filters:
                (!sectionId.HasValue               || doc.SectionId == sectionId.Value) &&
                (!departmentId.HasValue            || doc.Section!.DepartmentId == departmentId.Value) &&
                (!directorateId.HasValue           || doc.Section!.Department.DirectorateId == directorateId.Value) &&
                (!generalDirectorateId.HasValue    || doc.Section!.Department.Directorate.GeneralDirectorateId == generalDirectorateId.Value) &&
                (!ministryId.HasValue              || doc.Section!.Department.Directorate.GeneralDirectorate.MinistryId == ministryId.Value) &&

                // CC & Tag filtering
                (ccIds == null || !ccIds.Any()     || doc.CcLinks.Any(cl => ccIds.Contains(cl.DocumentCcId))) &&
                (tagIds == null || !tagIds.Any()   || doc.TagLinks.Any(tl => tagIds.Contains(tl.TagId)));
        }
    }
}
