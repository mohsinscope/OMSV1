// Domain/Entities/Documents/Document.cs
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Entities.Projects;
using OMSV1.Domain.Entities.Sections;
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


        public Guid? SectionId  { get; private set; }
        public Section? Section { get; private set; } = null!;
        public Guid? PrivatePartyId { get; private set; }
        public PrivateParty? PrivateParty { get; private set; }



        public Guid?      ParentDocumentId { get; private set; }
        public Document?  ParentDocument   { get; private set; }
        public ICollection<Document> ChildDocuments { get; private set; }

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

            Guid                profileId,
            Profile             profile,
            ResponseType        responseType,
            Guid                sectionId,
            Section             section,
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
            // PartyId         = partyId;
            // Party           = party;
            ProfileId       = profileId;
            Profile         = profile;
            ResponseType    = responseType;
            SectionId       = sectionId;
            Section         = section;
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
    string                        documentNumber,
    string                        title,
    DocumentType                  replyType,
    Guid                          projectId,
    DateTime                      replyDate,
    bool                          requiresReply,
    Guid                          profileId,
    Profile                       profile,
    ResponseType                  responseType,
    Guid                          sectionId,
    Section                       section,
    string?                       subject                  = null,
    bool                          isUrgent                 = false,
    bool                          isImportant              = false,
    bool                          isNeeded                 = false,
    IEnumerable<DocumentCC>?      ccs                      = null,
    IEnumerable<Tag>?             tags                     = null,
    string?                       notes                    = null
)
{
    var reply = new Document(
        documentNumber,
        title,
        replyType,
        projectId,
        replyDate,
        requiresReply,
        profileId,
        profile,
        responseType,
        sectionId,
        section,
        isUrgent,
        isImportant,
        isNeeded,
        subject,
        /* parentDocumentId: */ Id,
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
            string    title,
            string?   subject,
            DateTime  documentDate,
            DocumentType docType,
            bool      requiresReply,
            ResponseType responseType,
            bool      isUrgent,
            bool      isImportant,
            bool      isNeeded,
            string?   notes,
            Guid      projectId,
            Guid?     parentDocumentId,
            Guid      sectionId
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

            ProjectId       = projectId;
            ParentDocumentId= parentDocumentId;
            SectionId       = sectionId;
        }


        /* ─────────────── Patch Method ─────────────── */
        public void Patch(
            string?         documentNumber           = null,
            string?         title                    = null,
            string?         subject                  = null,
            DateTime?       documentDate             = null,
            DocumentType?   docType                  = null,
            bool?           requiresReply            = null,
            ResponseType?   responseType             = null,
            bool?           isUrgent                 = null,
            bool?           isImportant              = null,
            bool?           isNeeded                 = null,
            string?         notes                    = null,
            Guid?           projectId                = null,
            Guid?           parentDocumentId         = null,
            Guid?           sectionId                = null
        )
        {
            if (documentNumber  != null)     DocumentNumber         = documentNumber;
            if (title           != null)     Title                  = title;
            if (subject         != null)     Subject                = subject;
            if (documentDate.HasValue)       DocumentDate           = DateTime.SpecifyKind(documentDate.Value, DateTimeKind.Utc);
            if (docType.HasValue)            DocumentType           = docType.Value;
            if (requiresReply.HasValue)      IsRequiresReply        = requiresReply.Value;
            if (responseType.HasValue)       ResponseType           = responseType.Value;
            if (isUrgent.HasValue)           IsUrgent               = isUrgent.Value;
            if (isImportant.HasValue)        IsImportant            = isImportant.Value;
            if (isNeeded.HasValue)           IsNeeded               = isNeeded.Value;
            if (notes           != null)     Notes                  = notes;
            if (projectId.HasValue)          ProjectId              = projectId.Value;
            if (parentDocumentId.HasValue)   ParentDocumentId       = parentDocumentId;
            if (sectionId.HasValue)          SectionId              = sectionId.Value;
        }


        public void MarkAsReplied()             => IsReplied  = true;
        public void MarkAsAudited()             => IsAudited  = true;
        public void MarkNoLongerRequiresReply() => IsRequiresReply = false;
    }
}