// Domain/Entities/Documents/Document.cs
using System;
using System.Collections.Generic;
using System.Linq; // needed for Any()
using OMSV1.Domain.Entities.Ministries;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Entities.Projects;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Documents
{
    public class Document : Entity
    {
        /* ─────────────── Core fields ─────────────── */
        public string       DocumentNumber { get; private set; } = string.Empty;
        public string       Title          { get; private set; } = string.Empty;
        public DocumentType DocumentType   { get; private set; }
        public ResponseType ResponseType   { get; private set; }
        public string?      Subject        { get; private set; }

        /* ─────────────── Flags ─────────────── */
        public bool IsRequiresReply { get; private set; }
        public bool IsUrgent        { get; private set; }
        public bool IsImportant     { get; private set; }
        public bool IsNeeded        { get; private set; }

        /* ─────────────── Dates & notes ─────────────── */
        public DateTime DocumentDate { get; private set; }
        public string?  Notes        { get; private set; }

        /* ─────────────── Reply / audit status ─────────────── */
        public bool IsReplied { get; private set; }
        public bool IsAudited { get; private set; }

        /* ─────────────── FKs & navigations ─────────────── */
        public Guid     ProjectId  { get; private set; }
        public Project  Project    { get; private set; } = null!;

        public Guid?    MinistryId { get; private set; }
        public Ministry? Ministry  { get; private set; }

        public Guid?      ParentDocumentId { get; private set; }
        public Document?  ParentDocument   { get; private set; }
        public ICollection<Document> ChildDocuments { get; private set; }

        public Guid          PartyId { get; private set; }
        public DocumentParty Party   { get; private set; } = null!;

        public Guid    ProfileId { get; private set; }
        public Profile Profile   { get; private set; } = null!;

        /* ─────────────── CCs ─────────────── */
        public ICollection<DocumentCC> CCs { get; private set; }
        private readonly List<DocumentCcLink> _ccLinks = new();
        public IReadOnlyCollection<DocumentCcLink> CcLinks => _ccLinks.AsReadOnly();

        /* ─────────────── Tags ─────────────── */
        private readonly List<DocumentTagLink> _tagLinks = new();
        public IReadOnlyCollection<DocumentTagLink> TagLinks => _tagLinks.AsReadOnly();

        /* ─────────────── Constructors ─────────────── */
        protected Document()
        {
            ChildDocuments = new List<Document>();
            CCs            = new List<DocumentCC>();
            // Tags handled via TagLinks
        }

        public Document(
            string              documentNumber,
            string              title,
            DocumentType        docType,
            Guid                projectId,
            DateTime            documentDate,
            bool                requiresReply,
            Guid                partyId,
            DocumentParty       party,
            Guid                profileId,
            Profile             profile,
            ResponseType        responseType,
            Guid?               ministryId        = null,
            Ministry?           ministry          = null,
            bool                isUrgent          = false,
            bool                isImportant       = false,
            bool                isNeeded          = false,
            string?             subject           = null,
            Guid?               parentDocumentId  = null,
            IEnumerable<DocumentCC>? ccs          = null,
            IEnumerable<Tag>?   tags              = null,
            string?             notes             = null
        ) : this()
        {
            DocumentNumber  = documentNumber;
            Title           = title;
            DocumentType    = docType;
            ProjectId       = projectId;
            DocumentDate    = DateTime.SpecifyKind(documentDate, DateTimeKind.Utc);
            IsRequiresReply = requiresReply;
            PartyId         = partyId;
            Party           = party;
            ProfileId       = profileId;
            Profile         = profile;
            ResponseType    = responseType;
            MinistryId      = ministryId;
            Ministry        = ministry;
            IsUrgent        = isUrgent;
            IsImportant     = isImportant;
            IsNeeded        = isNeeded;
            Subject         = subject;
            ParentDocumentId= parentDocumentId;
            Notes           = notes;

            if (ccs != null)
                foreach (var cc in ccs)
                    AddCc(cc);

            if (tags != null)
                foreach (var t in tags)
                    AddTag(t);
        }

        public void AddCc(DocumentCC cc)
        {
            if (cc == null) throw new ArgumentNullException(nameof(cc));
            if (_ccLinks.Exists(l => l.DocumentCcId == cc.Id)) return;
            _ccLinks.Add(new DocumentCcLink(Id, cc.Id));
        }

        public void RemoveCc(Guid ccId) => _ccLinks.RemoveAll(l => l.DocumentCcId == ccId);

        public void AddTag(Tag tag)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            if (_tagLinks.Exists(l => l.TagId == tag.Id)) return;
            _tagLinks.Add(new DocumentTagLink(Id, tag.Id));
        }

        public void RemoveTag(Guid tagId) => _tagLinks.RemoveAll(l => l.TagId == tagId);

        public Document CreateReply(
            string              documentNumber,
            string              title,
            DocumentType        replyType,
            Guid                projectId,
            DateTime            replyDate,
            bool                requiresReply,
            Guid                partyId,
            DocumentParty       party,
            Guid                profileId,
            Profile             profile,
            ResponseType        responseType,
            Guid?               ministryId = null,
            Ministry?           ministry   = null,
            string?             subject    = null,
            bool                isUrgent   = false,
            bool                isImportant= false,
            bool                isNeeded   = false,
            IEnumerable<DocumentCC>? ccs   = null,
            IEnumerable<Tag>?   tags       = null,
            string?             notes      = null
        )
        {
            var reply = new Document(
                documentNumber,
                title,
                replyType,
                projectId,
                replyDate,
                requiresReply,
                partyId,
                party,
                profileId,
                profile,
                responseType,
                ministryId,
                ministry,
                isUrgent,
                isImportant,
                isNeeded,
                subject,
                Id,
                ccs,
                tags,
                notes
            );

            if (tags != null)
                foreach (var t in tags)
                    reply.AddTag(t);

            ChildDocuments.Add(reply);
            return reply;
        }

        public void Update(
            string       title,
            string?      subject,
            DateTime     documentDate,
            DocumentType docType,
            bool         requiresReply,
            ResponseType responseType,
            bool         isUrgent,
            bool         isImportant,
            bool         isNeeded,
            string?      notes = null
        )
        {
            Title           = title;
            Subject         = subject;
            DocumentDate    = DateTime.SpecifyKind(documentDate, DateTimeKind.Utc);
            DocumentType    = docType;
            IsRequiresReply = requiresReply;
            ResponseType    = responseType;
            IsUrgent        = isUrgent;
            IsImportant     = isImportant;
            IsNeeded        = isNeeded;
            Notes           = notes;
        }

        public void MarkAsReplied()             => IsReplied  = true;
        public void MarkAsAudited()             => IsAudited  = true;
        public void MarkNoLongerRequiresReply() => IsRequiresReply = false;
    }
}