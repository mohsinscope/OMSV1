// Domain/Entities/Documents/Document.cs
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.Entities.GeneralDirectorates;
using OMSV1.Domain.Entities.Ministries;
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
        //Project
        public Guid     ProjectId  { get; private set; }
        public Project  Project    { get; private set; } = null!;
        //Ministry
        public Guid? MinistryId  { get; private set; }
        public Ministry? Ministry { get; private set; } = null!;
        //GeneralDirectorate
        public Guid? GeneralDirectorateId  { get; private set; }
        public GeneralDirectorate? GeneralDirectorate { get; private set; } = null!;
        //Directorate
        public Guid? DirectorateId  { get; private set; }
        public Directorate? Directorate { get; private set; } = null!;
        //Department
        public Guid? DepartmentId  { get; private set; }
        public Department? Department { get; private set; } = null!;
        //Section
        public Guid? SectionId  { get; private set; }
        public Section? Section { get; private set; } = null!;
        //PrivateParty
        public Guid? PrivatePartyId { get; private set; }
        public PrivateParty? PrivateParty { get; private set; }

        public Guid?      ParentDocumentId { get; private set; }
        public Document?  ParentDocument   { get; private set; }
        public ICollection<Document> ChildDocuments { get; private set; }

        public Guid    ProfileId { get; private set; }
        public Profile Profile   { get; private set; } = null!;
        public ICollection<DocumentAttachment> DocumentAttachments { get; private set; }


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
    Guid?               ministryId,       // ← now nullable
    Ministry?            ministry,         // ← now nullable
    Guid?               generalDirectorateId,       // ← now nullable
    GeneralDirectorate?            generalDirectorate,         // ← now nullable
    Guid?               directorateId,       // ← now nullable
    Directorate?            directorate,         // ← now nullable
    Guid?               departmentId,       // ← now nullable
    Department?            department,         // ← now nullable
    Guid?               sectionId,       // ← now nullable
    Section?            section,         // ← now nullable
    Guid?               privatePartyId,  // ← still nullable
    PrivateParty?       privateParty,    // ← still nullable

    bool                isUrgent      = false,
    bool                isImportant   = false,
    bool                isNeeded      = false,
    string?             subject       = null,
    Guid?               parentDocumentId = null,
    IEnumerable<DocumentCC>? ccs      = null,
    IEnumerable<Tag>?   tags          = null,
    string?             notes         = null
) : this()
{
    DocumentNumber   = documentNumber;
    Title            = title;
    DocumentType     = docType;
    ProjectId        = projectId;
    DocumentDate     = DateTime.SpecifyKind(documentDate, DateTimeKind.Utc);
    IsRequiresReply  = requiresReply;

    ProfileId        = profileId;
    Profile          = profile;
    ResponseType     = responseType;
    
    MinistryId        = ministryId;
    Ministry          = ministry!;

    GeneralDirectorateId        = generalDirectorateId;
    GeneralDirectorate          = generalDirectorate!;

    DirectorateId        = directorateId;
    Directorate          = directorate!; 

    DepartmentId        = departmentId;
    Department          = department!; 
 
    SectionId        = sectionId;
    Section          = section!;           // will be null if sectionId was null

    PrivatePartyId   = privatePartyId;
    PrivateParty     = privateParty!;      // will be null if privatePartyId was null

    IsUrgent         = isUrgent;
    IsImportant      = isImportant;
    IsNeeded         = isNeeded;
    Subject          = subject;
    ParentDocumentId = parentDocumentId;
    Notes            = notes;

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

    Guid?               ministryId,       // ← now nullable
    Ministry?            ministry,         // ← now nullable
    Guid?               generalDirectorateId,       // ← now nullable
    GeneralDirectorate?            generalDirectorate,         // ← now nullable
    Guid?               directorateId,       // ← now nullable
    Directorate?            directorate,         // ← now nullable
    Guid?               departmentId,       // ← now nullable
    Department?            department,         // ← now nullable
    Guid?               sectionId,       // ← now nullable
    Section?            section,         // ← now nullable

    Guid?                         privatePartyId,  // ← still nullable
    PrivateParty?                 privateParty,    // ← still nullable

    string?                       subject           = null,
    bool                          isUrgent          = false,
    bool                          isImportant       = false,
    bool                          isNeeded          = false,
    IEnumerable<DocumentCC>?      ccs               = null,
    IEnumerable<Tag>?             tags              = null,
    string?                       notes             = null
)
{
    var reply = new Document(
        documentNumber:   documentNumber,
        title:            title,
        docType:          replyType,
        projectId:        projectId,
        documentDate:     replyDate,
        requiresReply:    requiresReply,
        profileId:        profileId,
        profile:          profile,
        responseType:     responseType,
        ministryId:       ministryId,
        ministry:         ministry,
        generalDirectorateId:       generalDirectorateId,
        generalDirectorate:         generalDirectorate,
        directorateId:       directorateId,
        directorate:         directorate,
        departmentId:       departmentId,
        department:         department,
        sectionId:        sectionId,        // nullable
        section:          section,          // nullable
        privatePartyId:   privatePartyId,   // nullable
        privateParty:     privateParty,     // nullable
        isUrgent:         isUrgent,
        isImportant:      isImportant,
        isNeeded:         isNeeded,
        subject:          subject,
        parentDocumentId: Id,               // parent is this document
        ccs:              ccs,
        tags:             tags,
        notes:            notes
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
            Guid?    parentDocumentId,
            Guid              ministryId,       // ← now nullable
            Guid              generalDirectorateId,       // ← now nullable
            Guid               directorateId,       // ← now nullable
            Guid               departmentId,       // ← now nullable
            Guid               sectionId,       // ← now nullable
            Guid      privatePartyId
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
            MinistryId        = ministryId;
            GeneralDirectorateId        = generalDirectorateId;
            DirectorateId        = directorateId;
            DepartmentId        = departmentId;
            SectionId       = sectionId;
            PrivatePartyId= privatePartyId;
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
            Guid?           ministryId               = null,
            Guid?           generalDirectorateId     = null,
            Guid?           directorateId            = null,
            Guid?           departmntId              = null,
            Guid?           sectionId                = null,
            Guid?           privatePartyId           = null
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
            
            if (ministryId.HasValue)          MinistryId              = ministryId.Value;
            if (generalDirectorateId.HasValue) GeneralDirectorateId              = generalDirectorateId.Value;
            if (directorateId.HasValue)          DirectorateId              = directorateId.Value;
            if (departmntId.HasValue)          DepartmentId              = departmntId.Value;
            if (sectionId.HasValue)          SectionId              = sectionId.Value;
            
            if (privatePartyId.HasValue)     PrivatePartyId         = privatePartyId.Value;

        }


        public void MarkAsReplied()             => IsReplied  = true;
        public void MarkAsAudited()             => IsAudited  = true;
        public void MarkNoLongerRequiresReply() => IsRequiresReply = false;
    }
}