using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Entities.Projects;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace OMSV1.Domain.Entities.Documents
{
    public class Document : Entity
    {
        // Unique Document Number.
        public string DocumentNumber { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        
        // Document type now only supports Incoming and Outgoing.
        public DocumentType DocumentType { get; private set; }

        // New property for Response Type.
        public ResponseType ResponseType { get; private set; }

        public string? Subject { get; private set; }
        
        // Indicates whether a reply is required.
        public bool IsRequiresReply { get; private set; }

        // Relationship to Project.
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; } = null!;  // null-forgiving operator

        // Self-reference for replies/confirmations.
        public Guid? ParentDocumentId { get; private set; }
        public Document? ParentDocument { get; private set; }
        public ICollection<Document> ChildDocuments { get; private set; }

        // Instead of a string, we use a full entity to represent the party (e.g., From, To, or CC)
        public Guid PartyId { get; private set; }
        public DocumentParty Party { get; private set; } = null!;

        // Updated: A document can now have many CC recipients.
        public ICollection<DocumentParty> CCs { get; private set; }

        public DateTime DocumentDate { get; private set; }

        // Attachments.
        public ICollection<AttachmentCU> Attachments { get; private set; }

        // New: Profile (main creator) relationship. Every document has a known creator.
        public Guid ProfileId { get; private set; }
        public Profile Profile { get; private set; } = null!;

        // New: Additional status flags.
        public bool IsReplied { get; private set; }
        public bool IsAudited { get; private set; }

        // EF / Serialization constructor.
        protected Document()
        {
            ChildDocuments = new List<Document>();
            Attachments = new List<AttachmentCU>();
            CCs = new List<DocumentParty>();
        }

        // Main constructor with optional CCs.
        public Document(
            string documentNumber,
            string title,
            DocumentType docType,
            Guid projectId,
            DateTime documentDate,
            bool requiresReply,
            Guid partyId,
            DocumentParty party,
            Guid profileId,
            Profile profile,
            ResponseType responseType,
            string? subject = null,
            Guid? parentDocumentId = null,
            IEnumerable<DocumentParty>? ccs = null
            ) : this()
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
            ProfileId = profileId;
            Profile = profile;
            
            // Now, no default is applied—the provided value is used.
            ResponseType = responseType;

            // If CCs provided, add them to the collection.
            if (ccs != null)
            {
                foreach(var cc in ccs)
                {
                    CCs.Add(cc);
                }
            }

            IsReplied = false;
            IsAudited = false;
        }

        // Domain method to create a reply document.
        public Document CreateReply(
            string documentNumber, // Use the supplied reply number.
            DocumentType replyType, 
            DateTime replyDate, 
            bool requiresReply, 
            Guid profileId, 
            Profile profile,
            ResponseType responseType,
            IEnumerable<DocumentParty>? ccs = null)
        {
            var reply = new Document(
                documentNumber: documentNumber,
                title: this.Title,
                docType: replyType,
                projectId: this.ProjectId,
                documentDate: replyDate,
                requiresReply: requiresReply,
                partyId: this.PartyId,
                party: this.Party,
                profileId: profileId,
                profile: profile,
                subject: this.Subject,
                parentDocumentId: this.Id,
                ccs: ccs,
                responseType: responseType
            );

            ChildDocuments.Add(reply);
            return reply;
        }

        /// <summary>
        /// Updates modifiable properties of the document.
        /// </summary>
        public void Update(string title, string? subject, DateTime documentDate,
                           DocumentType docType, bool isRequiresReply, ResponseType responseType)
        {
            Title = title;
            Subject = subject;
            DocumentDate = DateTime.SpecifyKind(documentDate, DateTimeKind.Utc);
            DocumentType = docType;
            IsRequiresReply = isRequiresReply;
            ResponseType = responseType;
        }

        public void ConfirmIncoming()
        {
            // For this revised model, you'll use the ResponseType to indicate reply/confirmation states.
            // E.g., you could set ResponseType = ResponseType.IncomingConfirmation.
        }

        public void MarkAsReplied()
        {
            IsReplied = true;
        }
        public void MarkAsAudited()
        {
            IsAudited = true;
        }


        public void MarkNoLongerRequiresReply()
        {
            IsRequiresReply = false;
        }

        // Additional methods to update IsReplied and IsAudited can be added as needed.
    }
}
