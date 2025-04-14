using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using System;

namespace OMSV1.Domain.Entities.DocumentHistories
{
    public class DocumentHistory : Entity
    {
        public Guid DocumentId { get; private set; }
        public Document Document { get; private set; } = null!;

        // Use the new enum DocumentActions
        public DocumentActions ActionType { get; private set; }

        // Use ProfileId and the associated Profile entity instead of UserId
        public Guid ProfileId { get; private set; }
        public Profile Profile { get; private set; } = null!;

        public DateTime ActionDate { get; private set; }
        public string? Notes { get; private set; }

        // EF / Serialization constructor
        protected DocumentHistory() { }

        public DocumentHistory(Guid documentId,
                               Guid profileId,
                               DocumentActions actionType,
                               DateTime actionDate,
                               string? notes = null)
        {
            DocumentId = documentId;
            ProfileId = profileId;
            ActionType = actionType;
            // Ensure UTC specification for DateTime
            ActionDate = DateTime.SpecifyKind(actionDate, DateTimeKind.Utc);
            Notes = notes;
        }
    }
}
