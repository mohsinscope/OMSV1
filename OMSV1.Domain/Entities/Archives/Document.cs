using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.Projects;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Documents
{
    public class Document : Entity
    {
        // Default initialization prevents CS8618 warnings.
        public string DocumentNumber { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public DocumentType DocumentType { get; private set; }
        public string? Subject { get; private set; }
        public bool IsRequiresReply { get; private set; }  // Indicates whether a reply is required

        // Relationship to Project
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; } = null!;  // null-forgiving operator

        // Self-reference for replies/confirmations
        public Guid? ParentDocumentId { get; private set; }
        public Document? ParentDocument { get; private set; }
        public ICollection<Document> ChildDocuments { get; private set; }

        // Instead of a string, we use a full entity to represent the party (e.g., From, To, or CC)
        public Guid PartyId { get; private set; }
        public DocumentParty Party { get; private set; } = null!;  // Assumes DocumentParty is an entity

        public Guid? CCId { get; private set; }
        public DocumentParty? CC { get; private set; }

        public DateTime DocumentDate { get; private set; }

        // Attachments
        public ICollection<AttachmentCU> Attachments { get; private set; }

        // EF / Serialization constructor
        protected Document()
        {
            ChildDocuments = new List<Document>();
            Attachments = new List<AttachmentCU>();
        }

        // Main constructor with optional CC parameters.
public Document(
    string documentNumber,
    string title,
    DocumentType docType,
    Guid projectId,
    DateTime documentDate,
    bool requiresReply,
    Guid partyId,
    DocumentParty party,
    string? subject = null,
    Guid? parentDocumentId = null,
    Guid? ccId = null,
    DocumentParty? cc = null)
    : this()
{
    DocumentNumber = documentNumber;
    Title = title;
    DocumentType = docType;
    ProjectId = projectId;
    DocumentDate = DateTime.SpecifyKind(documentDate, DateTimeKind.Utc);
    IsRequiresReply = requiresReply;
    PartyId = partyId;
    Party = party;
    Subject = subject;
    ParentDocumentId = parentDocumentId;

    // Use null if no CC is provided.
    if (ccId.HasValue && cc is not null)
    {
        CCId = ccId.Value;
        CC = cc;
    }
    else
    {
        // Assign null, not Guid.Empty.
        CCId = null;
        CC = null;
    }
}

        // Domain method to create a reply document.
        public Document CreateReply(DocumentType replyType, DateTime replyDate, bool requiresReply, Guid? ccId = null, DocumentParty? cc = null)
        {
            var reply = new Document(
                documentNumber: this.DocumentNumber, // or generate a new number if needed
                title: this.Title,
                docType: replyType,
                projectId: this.ProjectId,
                documentDate: replyDate,
                requiresReply: requiresReply,
                partyId: this.PartyId,
                party: this.Party,
                subject: this.Subject,
                parentDocumentId: this.Id,
                ccId: ccId,
                cc: cc
            );

            ChildDocuments.Add(reply);
            return reply;
        }


        public void ConfirmIncoming()
        {
            if (DocumentType == DocumentType.Incoming)
            {
                DocumentType = DocumentType.IncomingConfirmation;
            }
            // else handle other states, or throw if disallowed
        }

        public void MarkNoLongerRequiresReply()
        {
            IsRequiresReply = false;
        }

        

    }
}
