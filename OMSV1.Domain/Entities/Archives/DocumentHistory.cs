using OMSV1.Domain.Entities.Documents;
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

        // Which user performed this action
        public Guid UserId { get; private set; }
        // If you have a User entity:
        // public User User { get; private set; } = null!;

        public DateTime ActionDate { get; private set; }
        public string? Notes { get; private set; }

        // EF / Serialization constructor
        protected DocumentHistory() { }

        public DocumentHistory(Guid documentId,
                               Guid userId,
                               DocumentActions actionType,
                               DateTime actionDate,
                               string? notes = null)
        {
            DocumentId = documentId;
            UserId     = userId;
            ActionType = actionType;
            ActionDate = DateTime.SpecifyKind(actionDate, DateTimeKind.Utc);
            Notes      = notes;
        }
    }
}
