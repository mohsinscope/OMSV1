// using OMSV1.Domain.Entities.Projects;
// using OMSV1.Domain.Enums;
// using OMSV1.Domain.SeedWork;
// using System;

// namespace OMSV1.Domain.Entities.Documents
// {

//     public class DocumentParty : Entity
//     {
//         // Set default values
//         public string Name { get; private set; } = string.Empty;
//         public PartyType PartyType { get; private set; }
//         public bool IsOfficial { get; private set; }

//         // Foreign key to Project
//         public Guid ProjectId { get; private set; }
//         public Project Project { get; private set; } = null!;

//         // EF / Serialization constructor
//         protected DocumentParty() { }

//         // Main constructor
//         public DocumentParty(string name, PartyType partyType, bool isOfficial, Guid projectId)
//         {
//             if (string.IsNullOrWhiteSpace(name))
//                 throw new ArgumentException("Name cannot be null or empty.", nameof(name));
//             if (projectId == Guid.Empty)
//                 throw new ArgumentException("ProjectId must be a valid GUID.", nameof(projectId));

//             Name = name;
//             PartyType = partyType;
//             IsOfficial = isOfficial;
//             ProjectId = projectId;
//         }

//         public void UpdateName(string name)
//         {
//             if (string.IsNullOrWhiteSpace(name))
//                 throw new ArgumentException("Name cannot be null or empty.", nameof(name));
//             Name = name;
//         }

//         public void UpdatePartyType(PartyType partyType)
//         {
//             PartyType = partyType;
//         }

//         public void SetOfficial(bool isOfficial)
//         {
//             IsOfficial = isOfficial;
//         }

//         public void ChangeProject(Guid projectId)
//         {
//             if (projectId == Guid.Empty)
//                 throw new ArgumentException("ProjectId must be a valid GUID.", nameof(projectId));
//             ProjectId = projectId;
//         }
//     }
// }
