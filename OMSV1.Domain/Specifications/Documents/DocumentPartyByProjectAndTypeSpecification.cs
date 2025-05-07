// // Domain/Specifications/Documents/DocumentPartyByProjectAndTypeSpecification.cs
// using System;
// using OMSV1.Domain.Entities.Documents;
// using OMSV1.Domain.Enums;
// using OMSV1.Domain.SeedWork;

// namespace OMSV1.Domain.Specifications.Documents
// {
//     public class DocumentPartyByProjectAndTypeSpecification : BaseSpecification<DocumentParty>
//     {
//         public DocumentPartyByProjectAndTypeSpecification(
//             Guid projectId,
//             PartyType partyType,
//             bool isOfficial)
//             : base(dp =>
//                 dp.ProjectId   == projectId &&
//                 dp.PartyType   == partyType &&
//                 dp.IsOfficial  == isOfficial)
//         {
//         }
//     }
// }
